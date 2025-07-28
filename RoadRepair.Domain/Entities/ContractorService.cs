using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class ContractorService
    {
        public long Id { get; set; }
        public long TypeOfServiceId { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public long ContractorId { get; set; }
        public Contractor Contractor { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateOnly DateOfService { get; set; }
        public long WorkAreaId { get; set; }
        public WorkArea WorkArea { get; set; }
    }
}
