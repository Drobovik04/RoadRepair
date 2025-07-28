using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEventMedia.Commands.UpdateRepairEventMedia
{
    public class UpdateRepairEventMediaCommandHandler : IRequestHandler<UpdateRepairEventMediaCommand, ErrorOr<bool>>
    {
        private IRepairEventMediaRepository _repairEventMediaRepository;
        private IFileService _fileService;
        private IUnitOfWork _unitOfWork;
        public UpdateRepairEventMediaCommandHandler(IRepairEventMediaRepository repairEventMediaRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _repairEventMediaRepository = repairEventMediaRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateRepairEventMediaCommand request, CancellationToken cancellationToken)
        {
            var repairEventMedia = await _repairEventMediaRepository.GetRepairEventMediaByIdAsync(request.RepairEventMediaId);

            if (repairEventMedia == null)
            {
                return Error.NotFound(description: "RepairEventMedia not found");
            }

            string? oldFilePath = repairEventMedia.FilePath;
            string? newFilePath = null;

            if (request.File != null)
            {
                var uploadResult = await _fileService.Upload(request.File);

                if (uploadResult.IsError)
                {
                    return Error.Failure(description: "File upload failed");
                }

                newFilePath = uploadResult.Value;

                repairEventMedia.FilePath = newFilePath;
            }

            //var updateResult = await _fileService.Update(repairEventMedia.FilePath, request.File);

            //if (updateResult.IsError)
            //{
            //    return Error.Failure(description: "A failure has occured with updating file");
            //}

            repairEventMedia.RepairEventId = request.RepairEventId;
            //repairEventMedia.FilePath = updateResult.Value;
            repairEventMedia.CreatedAt = DateTime.UtcNow; // updated надо было бы может сделать, но пока так, будет обновлять дату
            repairEventMedia.Description = request.Description;

            _repairEventMediaRepository.UpdateRepairEventMedia(repairEventMedia);

            try
            {
                await _unitOfWork.CommitChangesAsync();

                // Обновление прошло успешно → удаляем старый файл
                if (request.File != null && oldFilePath != null)
                {
                    await _fileService.Delete(oldFilePath);
                }
            }
            catch (Exception ex)
            {
                // Откатываем загрузку нового файла, если он был
                if (newFilePath != null)
                {
                    await _fileService.Delete(newFilePath);
                }

                return Error.Failure(description: $"Database update failed: {ex.Message}");
            }

            return true;
        }
    }
}
