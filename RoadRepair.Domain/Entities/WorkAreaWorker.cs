using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class WorkAreaWorker
    {
        public long Id { get; set; }
        public long WorkAreaId { get; set; }
        public WorkArea WorkArea { get; set; }
        public long WorkerId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Worker Worker { get; set; }
        public List<WorkTime> WorkTimes { get; set; }
    }
}
