using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Repositories
{
    public class TypesOfMeasureRepository : ITypeOfMeasureRepository
    {
        private readonly AppDbContext _context;
        public TypesOfMeasureRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTypeOfMeasureAsync(TypeOfMeasure typeOfMeasure)
        {
            await _context.TypesOfMeasure.AddAsync(typeOfMeasure);
        }
        public async Task<TypeOfMeasure?> GetTypeOfMeasureByIdAsync(long typeOfMeasureId)
        {
            return await _context.TypesOfMeasure.FirstOrDefaultAsync(x => x.Id == typeOfMeasureId);
        }
        public async Task<IEnumerable<TypeOfMeasure>> GetAllTypesOfMeasureAsync()
        {
            return await _context.TypesOfMeasure.ToListAsync();
        }
        public void UpdateTypeOfMeasure(TypeOfMeasure typeOfMeasure)
        {
            _context.TypesOfMeasure.Update(typeOfMeasure);
        }

        public void DeleteTypeOfMeasure(TypeOfMeasure typeOfMeasure)
        {
            _context.TypesOfMeasure.Remove(typeOfMeasure);
        }
    }
}
