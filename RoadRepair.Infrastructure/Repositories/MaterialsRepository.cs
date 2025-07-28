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
    public class MaterialsRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;
        public MaterialsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddMaterialAsync(Material material)
        {
            await _context.Materials.AddAsync(material);
        }
        public async Task<Material?> GetMaterialByIdAsync(long materialId)
        {
            return await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialId);
        }
        public async Task<IEnumerable<Material>> GetAllMaterialsAsync(long organizationId)
        {
            return await _context.Materials.Include(x => x.TypeOfMeasure).Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
        public void UpdateMaterial(Material material)
        {
            _context.Materials.Update(material);
        }

        public void DeleteMaterial(Material material)
        {
            _context.Materials.Remove(material);
        }
    }
}
