using AutoMapper;
using ProjetPFE.Contracts;
using ProjetPFE.Contracts.services;
using ProjetPFE.Dto;

namespace ProjetPFE.Repository.service
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository employeRepository;
        private readonly IMapper map;

        public EmployeService(IEmployeRepository employeRepository, IMapper map)
        {
            this.employeRepository = employeRepository;
            this.map = map;
        }


        public async Task<ICollection<EmployeDto>> RetrieveEmployes()
        {
            var employes = await employeRepository.Getemployes();

            var EmployeDto = employes.Select(e => map.Map<EmployeDto>(e)).ToList();
            return EmployeDto;
        }

    }
}
