using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Domain.Entities
{
    public class Contractor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Email { get; set; }
        public string? ContactPhone { get; set; }
        public int UNP { get; set; }
        public List<MaterialSpend> MaterialSpends { get; set; }
        public List<ContractorService> ContractorServices { get; set; }

        //private Contractor() { }
        //public Contractor(long id, string name, string address, string? email, string? contactPhone, int unp, List<MaterialSpend> materialSpends, List<ContractorService> contractorServices)
        //{
        //    Id = id;
        //    Name = name;
        //    Address = address;
        //    Email = email;
        //    ContactPhone = contactPhone;
        //    UNP = unp;
        //    MaterialSpends = materialSpends;
        //    ContractorServices = contractorServices;
        //}
    }
}
