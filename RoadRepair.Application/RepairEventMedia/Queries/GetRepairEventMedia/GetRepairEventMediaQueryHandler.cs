using ErrorOr;
using MediatR;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Application.RepairEventMedia.Queries.GetRepairEventMedia
{
    public class GetRepairEventMediaQueryHandler : IRequestHandler<GetRepairEventMediaQuery, ErrorOr<Domain.Entities.RepairEventMedia>>
    {
        private IRepairEventMediaRepository _repairEventMediaRepository;
        public GetRepairEventMediaQueryHandler(IRepairEventMediaRepository repairEventMediaRepository)
        {
            _repairEventMediaRepository = repairEventMediaRepository;
        }

        public async Task<ErrorOr<Domain.Entities.RepairEventMedia>> Handle(GetRepairEventMediaQuery query, CancellationToken cancellationToken)
        {
            var repairEventMedia = await _repairEventMediaRepository.GetRepairEventMediaByIdAsync(query.RepairEventMediaId);

            return repairEventMedia is null
                ? Error.NotFound(description: "RepairEventMedia not found")
                : repairEventMedia;
        }
    }
}
