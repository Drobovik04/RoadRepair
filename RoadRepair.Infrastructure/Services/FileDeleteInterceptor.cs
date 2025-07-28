using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RoadRepair.Application.Interfaces;
using RoadRepair.Domain.Entities;

namespace RoadRepair.Infrastructure.Services
{
    public class FileDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public FileDeleteInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DeleteFiles(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DeleteFiles(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void DeleteFiles(DbContext context)
        {
            var deletedFiles = context.ChangeTracker.Entries<RepairEventMedia>()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity)
                .ToList();

            using (var scope = _serviceProvider.CreateScope())
            {
                var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();

                foreach (var path in deletedFiles)
                {
                    fileService.Delete(path.FilePath);
                }
            }
        }
    }

}
