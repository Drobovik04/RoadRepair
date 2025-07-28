using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using MediatR;

namespace RoadRepair.Application.WorkAreas.Commands.CreateWorkArea
{
    public class CreateWorkAreaCommandHandler : IRequestHandler<CreateWorkAreaCommand, ErrorOr<WorkArea>>
    {
        private IWorkAreaRepository _workAreaRepository;
        private IUnitOfWork _unitOfWork;
        public CreateWorkAreaCommandHandler(IWorkAreaRepository workAreaRepository, IUnitOfWork unitOfWork)
        {
            _workAreaRepository = workAreaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<WorkArea>> Handle(CreateWorkAreaCommand request, CancellationToken cancellationToken)
        {
            var workArea = new WorkArea
            {
                OrganizationId = request.OrganizationId,
                Name = request.Name,
                Description = request.Description,
                CreatedAt = request.CreatedAt,
                ResponsibleId = request.ResponsibleId,
            };

            await _workAreaRepository.AddWorkAreaAsync(workArea);

            await _unitOfWork.CommitChangesAsync();

            return workArea;
        }
    }
}
