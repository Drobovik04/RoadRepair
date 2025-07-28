using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using MediatR;
using RoadRepair.Application.Workers.Commands.UpdateWorker;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.WorkAreas.Commands.UpdateWorkArea
{
    public class UpdateWorkAreaCommandHandler : IRequestHandler<UpdateWorkAreaCommand, ErrorOr<bool>>
    {
        private IWorkAreaRepository _workAreaRepository;
        private IUnitOfWork _unitOfWork;
        public UpdateWorkAreaCommandHandler(IWorkAreaRepository workAreaRepository, IUnitOfWork unitOfWork)
        {
            _workAreaRepository = workAreaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(UpdateWorkAreaCommand request, CancellationToken cancellationToken)
        {
            var workArea = await _workAreaRepository.GetWorkAreaByIdAsync(request.WorkAreaId);

            if (workArea == null)
            {
                return Error.NotFound(description: "WorkArea not found");
            }

            // потом сделать может проверку на то, чтобы апдейт был больше create, пока пусть меняет в апдейте текущую, хотя, нужно тогда наверное менять время в зависимости от изменения вложений

            workArea.OrganizationId = request.OrganizationId;
            workArea.Name = request.Name;
            workArea.Description = request.Description;
            workArea.CreatedAt = request.CreatedAt;
            var now = DateTime.UtcNow;
            //workArea.UpdatedAt = new DateOnly(now.Year, now.Month, now.Day);
            workArea.UpdatedAt = request.UpdatedAt;
            workArea.ResponsibleId = request.ResponsibleId;

            _workAreaRepository.UpdateWorkArea(workArea);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
