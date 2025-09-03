using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Commands.DeleteRepairEventMedia
{
    public class DeleteRepairEventMediaCommandHandler : IRequestHandler<DeleteRepairEventMediaCommand, ErrorOr<bool>>
    {
        private IRepairEventMediaRepository _repairEventMediaRepository;
        private IFileService _fileService;
        private IUnitOfWork _unitOfWork;
        public DeleteRepairEventMediaCommandHandler(IRepairEventMediaRepository repairEventMediaRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _repairEventMediaRepository = repairEventMediaRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteRepairEventMediaCommand request, CancellationToken cancellationToken)
        {
            var repairEventMedia = await _repairEventMediaRepository.GetRepairEventMediaByIdAsync(request.Id);

            if (repairEventMedia == null)
            {
                return Error.NotFound(description: "RepairEvent not found");
            }

            var deleteResult = await _fileService.Delete(repairEventMedia.FilePath);

            if (deleteResult.IsError)
            {
                return deleteResult.FirstError;
            }

            _repairEventMediaRepository.DeleteRepairEventMedia(repairEventMedia);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
