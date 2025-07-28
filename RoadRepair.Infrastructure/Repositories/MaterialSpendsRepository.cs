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
    public class MaterialSpendsRepository : IMaterialSpendRepository
    {
        private readonly AppDbContext _context;
        public MaterialSpendsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddMaterialSpendAsync(MaterialSpend materialSpend)
        {
            await _context.MaterialSpends.AddAsync(materialSpend);
        }
        public async Task<MaterialSpend?> GetMaterialSpendByIdAsync(long materialSpendId)
        {
            return await _context.MaterialSpends.FirstOrDefaultAsync(x => x.Id == materialSpendId);
        }
        public async Task<IEnumerable<MaterialSpend>> GetAllMaterialSpendsAsync()
        {
            return await _context.MaterialSpends.ToListAsync();
        }
        public async Task<IEnumerable<MaterialSpend>> GetAllMaterialSpendsByRepairEventIdAsync(long repairEventId)
        {
            return await _context.MaterialSpends
                .Include(x => x.Material)
                    .ThenInclude(x => x.TypeOfMeasure)
                .Include(x => x.Contractor)
                .Where(x => x.RepairEventId == repairEventId).ToListAsync();
        }
        public void UpdateMaterialSpend(MaterialSpend materialSpend)
        {
            _context.MaterialSpends.Update(materialSpend);
        }

        public void DeleteMaterialSpend(MaterialSpend materialSpend)
        {
            _context.MaterialSpends.Remove(materialSpend);
        }
    }
}
