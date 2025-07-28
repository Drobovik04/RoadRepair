using RoadRepair.Application.Interfaces.Repositories;
using RoadRepair.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRepair.Application.Services
{
    //public class OrganizationService: IOrganizationsWriteService
    //{
    //    private readonly IOrganizationRepository _repository;
    //    public OrganizationService(IOrganizationRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public async Task<IEnumerable<OrganizationDto>> GetAllAsync()
    //    {
    //        var entities = await _repository.GetAllOrganizationsAsync();
    //        // Простое маппирование в DTO
    //        return entities.Select(e => new OrganizationDto
    //        {
    //            Id = e.Id,
    //            Name = e.Name,
    //            Address = e.Address
    //        });
    //    }

    //    public async Task<OrganizationDto> CreateAsync(OrganizationDto dto)
    //    {
    //        var entity = new Organization
    //        {
    //            Id = dto.Id == 0 ? 0 : dto.Id,
    //            Name = dto.Name,
    //            Address = dto.Address
    //        };
    //        await _repository.AddOrganizationAsync(entity);
    //        // Возвращаем созданный объект как DTO
    //        return new OrganizationDto { Id = entity.Id, Name = entity.Name, Address = entity.Address };
    //    }
    //}
}
