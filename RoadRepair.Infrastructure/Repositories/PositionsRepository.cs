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
    public class PositionsRepository : IPositionRepository
    {
        private readonly AppDbContext _context;
        public PositionsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddPositionAsync(Position positon)
        {
            await _context.Positions.AddAsync(positon);
        }
        public async Task<Position?> GetPositionByIdAsync(long positionId)
        {
            return await _context.Positions.FirstOrDefaultAsync(x => x.Id == positionId);
        }
        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }
        public void UpdatePosition(Position position)
        {
            _context.Positions.Update(position);
        }

        public void DeletePosition(Position position)
        {
            _context.Positions.Remove(position);
        }
    }
}
