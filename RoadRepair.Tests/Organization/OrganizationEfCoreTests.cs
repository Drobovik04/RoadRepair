using NUnit.Framework;
using RoadRepair.Tests.Database;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Tests.Organization
{
    [TestFixture]
    public class OrganizationEfCoreTests : TestDbContextFixture
    {
        [Test]
        public void CreateOrganization()
        {
            using var context = CreateContext();
            
            context.Organizations.Add(new RoadRepair.Domain.Entities.Organization() { Name = "TestOrg1"});
            context.SaveChanges();

            Assert.That(context.Organizations.First().Name == "TestOrg1");
        }
        [Test]
        public void UpdateOrganization()
        {
            using var context = CreateContext();

            context.Organizations.Add(new RoadRepair.Domain.Entities.Organization() { Name = "TestOrg1" });
            context.SaveChanges();

            context.Organizations.First().Name = "TestOrg12";

            Assert.That(context.Organizations.First().Name == "TestOrg12");
        }
        [Test]
        public void DeleteOrganizationAfterCreating() 
        {
            using var context = CreateContext();

            context.Organizations.Add(new RoadRepair.Domain.Entities.Organization() { Name = "TestOrg1" });
            context.SaveChanges();
            context.Organizations.Remove(context.Organizations.First());
            context.SaveChanges();

            Assert.That(context.Organizations.Count() == 0);
        }
        //[Test]
        //public void DeleteOrganizationAfterInsertingUserInOrganization()
        //{
        //    using var context = CreateContext();

        //    context.Organizations.Add(new RoadRepair.Domain.Entities.Organization() { Name = "TestOrg1" });
            
        //    context.SaveChanges();
        //    context.Organizations.First().AddUserToOrganization();

        //    Assert.That(context.Organizations.Count() == 0);
        //}
    }
}
