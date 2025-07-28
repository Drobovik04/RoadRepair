using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEventMedia.Commands.CreateRepairEventMedia
{
    public class CreateRepairEventCommandHandler : IRequestHandler<CreateRepairEventMediaCommand, ErrorOr<Domain.Entities.RepairEventMedia>>
    {
        private IRepairEventMediaRepository _repairEventMediaRepository;
        private IFileService _fileService;
        private IUnitOfWork _unitOfWork;
        public CreateRepairEventCommandHandler(IRepairEventMediaRepository repairEventMediaRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _repairEventMediaRepository = repairEventMediaRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Domain.Entities.RepairEventMedia>> Handle(CreateRepairEventMediaCommand request, CancellationToken cancellationToken)
        {
            var uploadResult = await _fileService.Upload(request.File);

            if (uploadResult.IsError)
            {
                return Error.Failure(description: "A failure has occured with attempt to save file");
            }

            var repairEventMedia = new Domain.Entities.RepairEventMedia
            {
                RepairEventId = request.RepairEventId,
                FilePath = uploadResult.Value,
                CreatedAt = DateTime.UtcNow,
                Description = request.Description,
            };

            await _repairEventMediaRepository.AddRepairEventMediaAsync(repairEventMedia);

            try
            {
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                await _fileService.Delete(uploadResult.Value);

                return Error.Failure(description: $"Database save failed: {ex.Message}");
            }

            await _unitOfWork.CommitChangesAsync();

            return repairEventMedia;
        }
    }
}
