using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Organizations.Commands.DeleteOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RoadRepair.Application.TypesOfRepair.Commands.DeleteTypeOfRepair
{
    public class DeleteTypeOfRepairCommandHandler : IRequestHandler<DeleteTypeOfRepairCommand, ErrorOr<bool>>
    {
        private ITypeOfRepairRepository _typeOfRepairRepository;
        private IUnitOfWork _unitOfWork;
        public DeleteTypeOfRepairCommandHandler(ITypeOfRepairRepository typeOfRepairRepository, IUnitOfWork unitOfWork)
        {
            _typeOfRepairRepository = typeOfRepairRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<bool>> Handle(DeleteTypeOfRepairCommand request, CancellationToken cancellationToken)
        {
            var typeOfRepair = await _typeOfRepairRepository.GetTypeOfRepairByIdAsync(request.Id);

            if (typeOfRepair == null)
            {
                return Error.NotFound(description: "TypeOfRepair not found");
            }

            _typeOfRepairRepository.DeleteTypeOfRepair(typeOfRepair);

            await _unitOfWork.CommitChangesAsync();

            return true;
        }
    }
}
