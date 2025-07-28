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
    public class TypesOfRepairRepository : ITypeOfRepairRepository
    {
        private readonly AppDbContext _context;
        public TypesOfRepairRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTypeOfRepairAsync(TypeOfRepair typeOfRepair)
        {
            await _context.TypesOfRepair.AddAsync(typeOfRepair);
        }
        public async Task<TypeOfRepair?> GetTypeOfRepairByIdAsync(long typeOfRepairId)
        {
            return await _context.TypesOfRepair.FirstOrDefaultAsync(x => x.Id == typeOfRepairId);
        }
        public async Task<IEnumerable<TypeOfRepair>> GetAllTypesOfRepairAsync()
        {
            return await _context.TypesOfRepair.ToListAsync();
        }
        public void UpdateTypeOfRepair(TypeOfRepair typeOfRepair)
        {
            _context.TypesOfRepair.Update(typeOfRepair);
        }

        public void DeleteTypeOfRepair(TypeOfRepair typeOfRepair)
        {
            _context.TypesOfRepair.Remove(typeOfRepair);
        }
    }
}
