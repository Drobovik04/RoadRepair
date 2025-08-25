using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkTimes.Commands.BulkUpdateWorkTime
{
    public class BulkUpdateWorkTimeCommandHandler : IRequestHandler<BulkUpdateWorkTimeCommand, ErrorOr<bool>>
    {
        private IWorkTimeRepository _workTimeRepository;
        private IWorkAreaWorkerRepository _workAreaWorkerRepository;
        private IUnitOfWork _unitOfWork;
        public BulkUpdateWorkTimeCommandHandler(IWorkTimeRepository workTimeRepository, IWorkAreaWorkerRepository workAreaWorkerRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _workAreaWorkerRepository = workAreaWorkerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(BulkUpdateWorkTimeCommand request, CancellationToken cancellationToken)
        {
            var workTimes = request.WorkTimes;

            if (workTimes == null || workTimes.Count == 0)
                return true;

            var workAreaWorkerIds = workTimes
                .Select(wt => wt.WorkAreaWorkerId)
                .Distinct()
                .ToList();

            if (workAreaWorkerIds == null)
            {
                return false;
            }

            // Получаем всех WorkAreaWorker с RepairEvent для проверки дат
            var workersWithEvents = (await _workAreaWorkerRepository.GetWorkAreaWorkersByIdsWithRepairEventAsync(workAreaWorkerIds)).ToList();

            if (workersWithEvents.Count != workAreaWorkerIds.Count)
                return Error.Failure("Не все работники найдены"); // Или другая ошибка

            // Создаём словарь для быстрой проверки диапазонов по WorkAreaWorkerId
            var workerDateRanges = workersWithEvents.ToDictionary(
                w => w.Id,
                w => 
                {
                    var events = w.WorkArea?.RepairZones?
                        .SelectMany(z => z.RepairEvents ?? Enumerable.Empty<RepairEvent>())
                        .ToList();

                    if (events == null || events.Count == 0)
                        return (Start: DateOnly.MinValue, End: DateOnly.MinValue);

                    return (
                        Start: events.Min(e => e.StartedAt),
                        End: events.Max(e => e.EndedAt ?? e.StartedAt)
                        //Start: w.WorkArea.RepairZones.Min(x => x.RepairEvents.Min(x => x.StartedAt)),
                        //End: w.WorkArea.RepairZones.Max(x => x.RepairEvents.Max(x => x.EndedAt))
                    );
                });

            // Проверяем, что даты WorkTime попадают в диапазон
            //foreach (var wt in workTimes)
            //{
            //    if (!workerDateRanges.TryGetValue(wt.WorkAreaWorkerId, out var range))
            //        return Error.Failure($"Работник {wt.WorkAreaWorkerId} не найден");

            //    if (wt.DayOfWork < range.Start || wt.DayOfWork > range.End)
            //        return Error.Failure($"Дата {wt.DayOfWork:yyyy-MM-dd} вне диапазона ремонта для работника {wt.WorkAreaWorkerId}");
            //}

            var errors = new List<string>();

            // Отфильтровываем только валидные WorkTime
            var validWorkTimes = new List<WorkTime>();

            foreach (var wt in workTimes)
            {
                if (!workerDateRanges.TryGetValue(wt.WorkAreaWorkerId, out var range))
                {
                    errors.Add($"Работник {wt.WorkAreaWorkerId} не найден");
                    continue; // пропускаем
                }

                if (wt.DayOfWork < range.Start || wt.DayOfWork > range.End)
                {
                    errors.Add($"Дата {wt.DayOfWork:yyyy-MM-dd} вне диапазона ремонта для работника {wt.WorkAreaWorkerId}");
                    continue; // пропускаем
                }

                validWorkTimes.Add(wt);
            }

            // Получаем все существующие записи для работников
            var existing = new List<WorkTime>();
            foreach (var id in workAreaWorkerIds)
            {
                existing.AddRange(await _workTimeRepository.GetAllWorkTimesByWorkAreaWorkerIdAsync(id));
            }

            // Удаляем WorkTime вне диапазона дат RepairEvent для каждого работника
            var toRemove = existing.Where(wt =>
            {
                if (workerDateRanges.TryGetValue(wt.WorkAreaWorkerId, out var range))
                {
                    return wt.DayOfWork < range.Start || wt.DayOfWork > range.End;
                }
                return false;
            }).ToList();

            foreach (var rem in toRemove)
                _workTimeRepository.DeleteWorkTime(rem);

            // Обновляем / добавляем WorkTime (дубликаты удаляем, оставляем один)
            var existingMap = existing
                .Where(wt => !toRemove.Contains(wt)) // исключаем удалённые
                .GroupBy(e => (e.WorkAreaWorkerId, e.DayOfWork))
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.Id).ToList()
                );

            foreach (var wt in validWorkTimes)
            {
                var key = (wt.WorkAreaWorkerId, wt.DayOfWork);
                if (existingMap.TryGetValue(key, out var entries))
                {
                    // Удаляем дубликаты
                    for (int i = 1; i < entries.Count; i++)
                        _workTimeRepository.DeleteWorkTime(entries[i]);

                    // Обновляем первый
                    entries[0].Hours = wt.Hours;
                }
                else
                {
                    // Добавляем новую запись
                    await _workTimeRepository.AddWorkTimeAsync(new WorkTime
                    {
                        WorkAreaWorkerId = wt.WorkAreaWorkerId,
                        DayOfWork = wt.DayOfWork,
                        Hours = wt.Hours
                    });
                }
            }

            // Удаляем записи, которых нет в новом списке (в пределах диапазона)
            var validKeys = validWorkTimes.Select(wt => (wt.WorkAreaWorkerId, wt.DayOfWork)).ToHashSet();

            foreach (var exist in existing)
            {
                var key = (exist.WorkAreaWorkerId, exist.DayOfWork);

                // Удаляем если нет в validKeys И если дата входит в диапазон (иначе уже удалено)
                if (!validKeys.Contains(key) &&
                    workerDateRanges.TryGetValue(exist.WorkAreaWorkerId, out var range) &&
                    exist.DayOfWork >= range.Start && exist.DayOfWork <= range.End)
                {
                    _workTimeRepository.DeleteWorkTime(exist);
                }
            }

            //// Получаем все существующие записи для всех переданных работников
            //var existing = new List<WorkTime>();

            //for (int i = 0; i < workAreaWorkerIds.Count; i++)
            //{
            //    existing.AddRange(await _workTimeRepository.GetAllWorkTimesByWorkAreaWorkerIdAsync(workAreaWorkerIds[i]));
            //}

            //// Преобразуем в словарь (key = workerId + date) для быстрого доступа
            //var existingMap = existing
            //    .GroupBy(e => (e.WorkAreaWorkerId, e.DayOfWork))
            //    .ToDictionary(
            //        g => g.Key,
            //        g => g.OrderBy(x => x.Id).ToList()
            //    );

            //// Обработка: добавление, обновление
            //foreach (var wt in workTimes)
            //{
            //    var key = (wt.WorkAreaWorkerId, wt.DayOfWork);

            //    if (existingMap.TryGetValue(key, out var entries))
            //    {
            //        // Удаляем дубликаты, оставляем один
            //        for (int i = 1; i < entries.Count; i++)
            //            _workTimeRepository.DeleteWorkTime(entries[i]);

            //        // Обновляем первый
            //        entries[0].Hours = wt.Hours;
            //    }
            //    else
            //    {
            //        // Добавляем новую запись
            //        await _workTimeRepository.AddWorkTimeAsync(new WorkTime
            //        {
            //            WorkAreaWorkerId = wt.WorkAreaWorkerId,
            //            DayOfWork = wt.DayOfWork,
            //            Hours = wt.Hours
            //        });
            //    }
            //}

            //// Удаление записей, которых нет в новом списке
            //var validKeys = workTimes.Select(wt => (wt.WorkAreaWorkerId, wt.DayOfWork)).ToHashSet();

            //foreach (var existingEntry in existing)
            //{
            //    var key = (existingEntry.WorkAreaWorkerId, existingEntry.DayOfWork);
            //    if (!validKeys.Contains(key))
            //    {
            //        _workTimeRepository.DeleteWorkTime(existingEntry);
            //    }
            //}

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
