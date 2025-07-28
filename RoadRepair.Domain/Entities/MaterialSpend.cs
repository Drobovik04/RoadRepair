using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class MaterialSpend
    {
        public long Id { get; set; }
        public long MaterialId { get; set; }
        public Material Material { get; set; }
        public decimal Price { get; set; }
        public double Volume { get; set; }
        public long RepairEventId { get; set; }
        public RepairEvent RepairEvent { get; set; }
        public long ContractorId { get; set; }
        public Contractor Contractor { get; set; }
    }
}
