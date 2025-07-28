using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Interfaces.Repositories
{
    public interface ITypeOfMeasureRepository
    {
        Task<IEnumerable<TypeOfMeasure>> GetAllTypesOfMeasureAsync();
        Task<TypeOfMeasure?> GetTypeOfMeasureByIdAsync(long id);
        Task AddTypeOfMeasureAsync(TypeOfMeasure typeOfMeasure);
        void UpdateTypeOfMeasure(TypeOfMeasure typeOfMeasure);
        void DeleteTypeOfMeasure(TypeOfMeasure typeOfMeasure);
    }
}
