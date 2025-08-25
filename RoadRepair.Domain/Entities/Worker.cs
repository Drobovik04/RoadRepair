using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class Worker
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateOnly HiredAt { get; set; }
        public DateOnly? FiredAt { get; set; }
        public long? PositionId { get; set; }
        public Position? Position { get; set; }
        public List<WorkAreaWorker> WorkAreaWorkers { get; set; }
        
    }
}
