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
        private IUnitOfWork _unitOfWork;
        public BulkUpdateWorkTimeCommandHandler(IWorkTimeRepository workTimeRepository, IUnitOfWork unitOfWork)
        {
            _workTimeRepository = workTimeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(BulkUpdateWorkTimeCommand request, CancellationToken cancellationToken)
        {

            //var existing = await _workTimeRepository.GetAllWorkTimesByRepairEventWorkerIdAsync(request.WorkTimes.FirstOrDefault().Id);

            //// Удаляем те, которых нет в новом списке
            //var toDelete = existing
            //    .Where(e => !request.WorkTimes.Any(n =>
            //        n.RepairEventWorkerId == e.RepairEventWorkerId &&
            //        n.DayOfWork == e.DayOfWork))
            //    .ToList();

            //for (int i = 0; i < toDelete.Count; i++) 
            //{
            //    _workTimeRepository.DeleteWorkTime(toDelete[i]);
            //}

            //foreach (var wt in request.WorkTimes)
            //{
            //    var existingRecord = existing.FirstOrDefault(e =>
            //        e.RepairEventWorkerId == wt.RepairEventWorkerId &&
            //        e.DayOfWork == wt.DayOfWork);

            //    if (existingRecord != null)
            //    {
            //        // Обновляем
            //        existingRecord.Hours = wt.Hours;
            //    }
            //    else
            //    {
            //        // Добавляем
            //        await _workTimeRepository.AddWorkTimeAsync(new WorkTime
            //        {
            //            RepairEventWorkerId = wt.RepairEventWorkerId,
            //            DayOfWork = wt.DayOfWork,
            //            Hours = wt.Hours
            //        });
            //    }
            //}

            var workTimes = request.WorkTimes;

            if (workTimes == null || workTimes.Count == 0)
                return true;

            var workAreaWorkerIds = workTimes
                .Select(wt => wt.WorkAreaWorkerId)
                .Distinct()
                .ToList();

            // Получаем все существующие записи для всех переданных работников
            var existing = new List<WorkTime>();

            for (int i = 0; i < workAreaWorkerIds.Count; i++)
            {
                existing.AddRange(await _workTimeRepository.GetAllWorkTimesByWorkAreaWorkerIdAsync(workAreaWorkerIds[i]));
            }

            // Преобразуем в словарь (key = workerId + date) для быстрого доступа
            var existingMap = existing
                .GroupBy(e => (e.WorkAreaWorkerId, e.DayOfWork))
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(x => x.Id).ToList()
                );

            // Обработка: добавление, обновление
            foreach (var wt in workTimes)
            {
                var key = (wt.WorkAreaWorkerId, wt.DayOfWork);

                if (existingMap.TryGetValue(key, out var entries))
                {
                    // Удаляем дубликаты, оставляем один
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

            // Удаление записей, которых нет в новом списке
            var validKeys = workTimes.Select(wt => (wt.WorkAreaWorkerId, wt.DayOfWork)).ToHashSet();

            foreach (var existingEntry in existing)
            {
                var key = (existingEntry.WorkAreaWorkerId, existingEntry.DayOfWork);
                if (!validKeys.Contains(key))
                {
                    _workTimeRepository.DeleteWorkTime(existingEntry);
                }
            }

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
