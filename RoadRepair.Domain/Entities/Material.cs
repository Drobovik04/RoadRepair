using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class Material
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long TypeOfMeasureId { get; set; }
        public TypeOfMeasure TypeOfMeasure { get; set; }
        public List<MaterialSpend> MaterialSpends { get; set; }
    }
}
