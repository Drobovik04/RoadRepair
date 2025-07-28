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
    public class TypesOfServiceRepository : ITypeOfServiceRepository
    {
        private readonly AppDbContext _context;
        public TypesOfServiceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTypeOfServiceAsync(TypeOfService typesOfService)
        {
            await _context.TypesOfService.AddAsync(typesOfService);
        }
        public async Task<TypeOfService?> GetTypeOfServiceByIdAsync(long typesOfServiceId)
        {
            return await _context.TypesOfService.FirstOrDefaultAsync(x => x.Id == typesOfServiceId);
        }
        public async Task<IEnumerable<TypeOfService>> GetAllTypesOfServiceAsync()
        {
            return await _context.TypesOfService.ToListAsync();
        }
        public void UpdateTypeOfService(TypeOfService typesOfService)
        {
            _context.TypesOfService.Update(typesOfService);
        }

        public void DeleteTypeOfService(TypeOfService typesOfService)
        {
            _context.TypesOfService.Remove(typesOfService);
        }
    }
}
