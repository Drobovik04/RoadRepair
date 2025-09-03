using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoadRepair.Application.Interfaces;
using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using RoadRepair.Infrastructure.Data;
using RoadRepair.Infrastructure.Identity;
using RoadRepair.Infrastructure.Repositories;
using RoadRepair.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace RoadRepair.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<FileDeleteInterceptor>();

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<FileDeleteInterceptor>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opts => opts.MigrationsAssembly("RoadRepair.Infrastructure"));
                options.AddInterceptors(interceptor);
            });

            services.AddIdentity<AppUser, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddAuthorization();

            //Repositories and etc.
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IPositionRepository, PositionsRepository>();
            services.AddScoped<IWorkTimeRepository, WorkTimesRepository>();
            services.AddScoped<ITypeOfRepairRepository, TypesOfRepairRepository>();
            services.AddScoped<ITypeOfServiceRepository, TypesOfServiceRepository>();
            services.AddScoped<ITypeOfMeasureRepository, TypesOfMeasureRepository>();
            services.AddScoped<IWorkerRepository, WorkersRepository>();
            services.AddScoped<IMaterialRepository, MaterialsRepository>();
            services.AddScoped<IContractorRepository, ContractorsRepository>();
            services.AddScoped<IContractorServiceRepository, ContractorServicesRepository>();
            services.AddScoped<IMaterialSpendRepository, MaterialSpendsRepository>();
            services.AddScoped<IRepairZoneRepository, RepairZonesRepository>();
            services.AddScoped<IWorkAreaRepository, WorkAreasRepository>();
            services.AddScoped<IRepairEventRepository, RepairEventsRepository>();
            services.AddScoped<IRepairEventMediaRepository, RepairEventMediaRepository>();
            services.AddScoped<IWorkAreaWorkerRepository, WorkAreaWorkersRepository>();

            services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}