using AutoMapper;
using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Context;

public class DapperProfile : Profile
{
    public DapperProfile()
    {
        CreateMap<employe, EmployeDto>();
    }
}
