using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>, IUnitOfWork
    {
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<ContractorService> ContractorServices { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialSpend> MaterialSpends { get; set; }
        public DbSet<TypeOfMeasure> TypesOfMeasure { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<RepairEvent> RepairEvents { get; set; }
        public DbSet<RepairEventMedia> RepairEventMedia { get; set; }
        public DbSet<WorkAreaWorker> WorkAreaWorkers { get; set; }
        public DbSet<RepairZone> RepairZones { get; set; }
        //public new DbSet<Role> Roles { get; set; } // может ошибка будет из-за перекрытия ролей edentity пока удалил, identity их реализует
        public DbSet<TypeOfRepair> TypesOfRepair { get; set; }
        public DbSet<TypeOfService> TypesOfService { get; set; }
        public DbSet<WorkArea> WorkAreas { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkTime> WorkTimes { get; set; }
        public DbSet<User> UsersInfo { get; set; }
        //User нету, пока без него, будем через appuser работать, хз

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        // Настройка схемы, при необходимости
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Можно указывать связи, ограничения, индексы и т.д.

            // Many to many
            // MaterialSpend
            // Материал с контрагентами и repairevent
            modelBuilder.Entity<MaterialSpend>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MaterialSpend>()
                .HasOne(x => x.Material)
                .WithMany(x => x.MaterialSpends)
                .HasForeignKey(x => x.MaterialId);

            modelBuilder.Entity<MaterialSpend>()
                .HasOne(x => x.Contractor)
                .WithMany(x => x.MaterialSpends)
                .HasForeignKey(x => x.ContractorId);

            modelBuilder.Entity<MaterialSpend>()
                .HasOne(x => x.RepairEvent)
                .WithMany(x => x.MaterialSpends)
                .HasForeignKey(x => x.RepairEventId);

            modelBuilder.Entity<MaterialSpend>()
                .Property(x => x.Price)
                .HasPrecision(18,2);

            // ContractorService

            modelBuilder.Entity<ContractorService>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ContractorService>()
                .HasOne(x => x.TypeOfService)
                .WithMany(x => x.ContractorServices)
                .HasForeignKey(x => x.TypeOfServiceId);

            modelBuilder.Entity<ContractorService>()
                .HasOne(x => x.Contractor)
                .WithMany(x => x.ContractorServices)
                .HasForeignKey(x => x.ContractorId);

            modelBuilder.Entity<ContractorService>()
                .HasOne(x => x.WorkArea)
                .WithMany(x => x.ContractorServices)
                .HasForeignKey(x => x.WorkAreaId);

            modelBuilder.Entity<ContractorService>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            // WorkAreaWorker

            modelBuilder.Entity<WorkAreaWorker>()
                .HasOne(x => x.Worker)
                .WithMany(x => x.WorkAreaWorkers)
                .HasForeignKey(x => x.WorkerId);

            modelBuilder.Entity<WorkAreaWorker>()
                .HasOne(x => x.WorkArea)
                .WithMany(x => x.WorkAreaWorkers)
                .HasForeignKey(x => x.WorkAreaId);


            modelBuilder.Entity<RepairEventMedia>()
                .HasOne(x => x.RepairEvent)
                .WithMany(x => x.RepairEventMedia)
                .HasForeignKey(x => x.RepairEventId)
                .OnDelete(DeleteBehavior.Cascade);

            // TypeOfMeasure
            modelBuilder.Entity<TypeOfMeasure>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // TypeOfService
            modelBuilder.Entity<TypeOfService>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // TypeOfRepair
            modelBuilder.Entity<TypeOfRepair>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // Positions
            modelBuilder.Entity<Position>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // WorkTime
            modelBuilder.Entity<WorkTime>()
                .HasIndex(wt => new { wt.WorkAreaWorkerId, wt.DayOfWork })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<User>(x => x.IdentityId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
