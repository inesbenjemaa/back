using ProjetPFE.Dto;

namespace ProjetPFE.Contracts.services
{
    public interface IEmployeService
    {
        public Task<ICollection<EmployeDto>> RetrieveEmployes();

    }
}
