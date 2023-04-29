using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Contracts
{
    public interface IDiplomeRepository
    {
        public Task<IEnumerable<diplome>> GetDiplomes();
        public Task<diplome> GetDiplome(int diplome_id);
        public Task<diplome> CreateDiplome(DiplomeForCreationDto diplome);
        public Task UpdateDiplome(int diplome_id, DiplomeForUpdateDto   diplome);
        public Task DeleteDiplome(int diplome_id);
    }
}
