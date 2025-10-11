using ErrorOr;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Errors;

namespace RoadRepair.Application.TypesOfMeasure.Commands.CreateTypeOfMeasure
{
    public class CreateTypeOfMeasureCommandHandler : IRequestHandler<CreateTypeOfMeasureCommand, ErrorOr<TypeOfMeasure>>
    {
        private ITypeOfMeasureRepository _typeOfMeasureRepository;
        private IUnitOfWork _unitOfWork;
        public CreateTypeOfMeasureCommandHandler(ITypeOfMeasureRepository typeOfMeasureRepository, IUnitOfWork unitOfWork)
        {
            _typeOfMeasureRepository = typeOfMeasureRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<TypeOfMeasure>> Handle(CreateTypeOfMeasureCommand request, CancellationToken cancellationToken)
        {
            //try
            //{
                var typeOfMeasure = new TypeOfMeasure
                {
                    Name = request.Name,
                    ShortName = request.ShortName,
                };

                await _typeOfMeasureRepository.AddTypeOfMeasureAsync(typeOfMeasure);

                await _unitOfWork.CommitChangesAsync();

                return typeOfMeasure;
            //}
            //catch (DbUpdateException ex)
            //{
            //    var msg = ex.InnerException?.Message ?? ex.Message;

            //    if (msg.Contains("повтор", StringComparison.OrdinalIgnoreCase))
            //    {
            //        return Error.Failure(description: ErrorsDictionary.GetErrorString(ErrorTypes.UniqueError));
            //    }

            //    if (msg.Contains("NULL", StringComparison.OrdinalIgnoreCase))
            //    {
            //        return Error.Failure(description: ErrorsDictionary.GetErrorString(ErrorTypes.NullError));
            //    }

            //    return Error.Failure(description: $"Ошибка при сохранении в БД: {msg}");
            //}
            //catch (Exception ex)
            //{
            //    return Error.Failure(description: $"Ошибка при сохранении: {ex.Message}");
            //}
        }
    }
}
