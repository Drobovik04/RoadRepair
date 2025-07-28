using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//СДЕЛАЛ ССЫЛКУ НА INFRASTRUCTURE ради нормальной логики, сори (уже убрал)


namespace RoadRepair.Domain.Entities
{
    public class Organization
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public int UNP { get; set; }
        public long? ParentId { get; set; }
        public Organization? Parent { get; set; }
        public List<User> Users { get; set; } = new();
        public List<WorkArea> WorkAreas { get; set; } = new();
        public List<Material> Materials { get; set; } = new(); // для отделения материалов между несколькими организациями
        public List<Worker> Workers { get; set; } = new();

        public Organization() { }
        public Organization(string name, string? address, string? contactPhone, string? email, int unp, long? parentId, List<User>? users, List<WorkArea>? workAreas, List<Material> materials, List<Worker> workers) 
        { 
            Name = name;
            Address = address;
            ContactPhone = contactPhone;
            Email = email;
            UNP = unp;
            ParentId = parentId;

            if (users != null) Users = users;
            if (workAreas != null) WorkAreas = workAreas;
            if (materials != null) Materials = materials;
            if (workers != null) Workers = workers;
        }
        public void AddUserToOrganization(User user)
        {
            Users.Add(user);
            user.Organization = this;
        }
        public void RemoveUserFromOrganization(User user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
                user.Organization = null;
            }
        }
    }
}
