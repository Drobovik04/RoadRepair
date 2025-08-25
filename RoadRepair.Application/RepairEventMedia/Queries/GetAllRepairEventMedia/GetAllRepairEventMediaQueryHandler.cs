using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.RepairEventMedia.Queries.GetAllRepairMedia
{
    public class GetAllRepairEventMediaQueryHandler : IRequestHandler<GetAllRepairEventMediaQuery, ErrorOr<List<Domain.Entities.RepairEventMedia>>>
    {
        private IRepairEventMediaRepository _repairEventMediaRepository;
        private IFileService _fileService;
        public GetAllRepairEventMediaQueryHandler(IRepairEventMediaRepository repairEventMediaRepository, IFileService fileService)
        {
            _repairEventMediaRepository = repairEventMediaRepository;
            _fileService = fileService;
        }

        public async Task<ErrorOr<List<Domain.Entities.RepairEventMedia>>> Handle(GetAllRepairEventMediaQuery query, CancellationToken cancellationToken)
        {
            var repairEventMedia = await _repairEventMediaRepository.GetAllRepairEventMediaByRepairZoneIdAsync(query.repairEventId);

            return repairEventMedia is null
                ? Error.NotFound(description: "RepairEventMedia are not found")
                : repairEventMedia.ToList();
        }
    }
}
