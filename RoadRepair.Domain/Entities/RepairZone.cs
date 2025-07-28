using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class RepairZone
    {
        public long Id { get; set; }
        public long WorkAreaId { get; set; }
        public WorkArea WorkArea { get; set; }
        public string Name { get; set; }
        public string? GeometryJson { get; set; }
        public List<RepairEvent> RepairEvents { get; set; }

        // не знаю для чего я хотел их добавить, честно), created и modified еще можно, но не это
        //public DateTime? ValidFrom { get; set; }
        //public DateTime? ValidTo { get; set;}
    }
}
