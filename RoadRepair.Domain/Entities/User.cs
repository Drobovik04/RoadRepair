using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RoadRepair.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FirstName { get; set; }
        //public long? RoleId { get; set; }
        //public Role? Role { get; set; }
        public long IdentityId { get; set; }
        public DateTime CreatedAt { get; set; }
        // хизи, пока пусть AppUser побудет

        public User() { }
        public User(string lastName, string? middleName, string firstName, long identityId)
        {
            LastName = lastName;
            FirstName = firstName;
            IdentityId = identityId;

            if (middleName != null) MiddleName = middleName;
        }
    }
}
