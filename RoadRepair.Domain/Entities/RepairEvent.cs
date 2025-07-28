using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class RepairEvent
    {
        public long Id { get; set; }
        public long RepairZoneId { get; set; }
        public RepairZone RepairZone { get; set; }
        public DateOnly StartedAt { get; set; }
        public DateOnly? EndedAt { get; set; }
        public long TypeOfRepairId { get; set; }
        public TypeOfRepair TypeOfRepair { get; set; }
        public List<RepairEventMedia> RepairEventMedia { get; set; }
        public List<MaterialSpend> MaterialSpends { get; set; }
        public List<RepairEventWorker> RepairEventWorkers { get; set; }
    }
}
