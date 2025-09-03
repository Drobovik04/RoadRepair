using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class WorkArea
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public DateOnly? UpdatedAt { get; set; }
        public long? ResponsibleId { get; set; }
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Worker? Responsible {  get; set; }
        public List<ContractorService> ContractorServices { get; set; }
        public List<RepairZone> RepairZones { get; set; }
        public List<WorkAreaWorker> WorkAreaWorkers { get; set; }
    }
}
