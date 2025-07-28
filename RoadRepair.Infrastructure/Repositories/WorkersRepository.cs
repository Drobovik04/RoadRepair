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
    public class WorkersRepository : IWorkerRepository
    {
        private readonly AppDbContext _context;
        public WorkersRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddWorkerAsync(Worker worker)
        {
            await _context.Workers.AddAsync(worker);
        }
        public async Task<Worker?> GetWorkerByIdAsync(long workerId)
        {
            return await _context.Workers.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == workerId);
        }
        public async Task<IEnumerable<Worker>> GetAllWorkersAsync()
        {
            return await _context.Workers.Include(x => x.Position).ToListAsync();
        }
        public void UpdateWorker(Worker worker)
        {
            _context.Workers.Update(worker);
        }

        public void DeleteWorker(Worker worker)
        {
            _context.Workers.Remove(worker);
        }
    }
}
