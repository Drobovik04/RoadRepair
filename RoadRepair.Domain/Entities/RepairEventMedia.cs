using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class RepairEventMedia
    {
        public long Id { get; set; }
        public long RepairEventId { get; set; }
        public RepairEvent RepairEvent { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}
