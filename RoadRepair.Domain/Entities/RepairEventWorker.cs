using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class RepairEventWorker
    {
        public long Id { get; set; }
        public long RepairEventId { get; set; }
        public RepairEvent RepairEvent { get; set; }
        public long WorkerId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Worker Worker { get; set; }
        public List<WorkTime> WorkTimes { get; set; }
    }
}
