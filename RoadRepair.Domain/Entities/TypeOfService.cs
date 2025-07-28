using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class TypeOfService
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<ContractorService> ContractorServices { get; set; }
    }
}
