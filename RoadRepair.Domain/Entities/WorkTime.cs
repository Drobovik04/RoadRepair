using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class WorkTime
    {
        public long Id { get; set; }
        public DateOnly DayOfWork { get; set; }
        public double Hours { get; set; }
        public long RepairEventWorkerId { get; set; }
        public RepairEventWorker RepairEventWorker { get; set; }
    }
}
