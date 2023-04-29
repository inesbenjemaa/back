using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Contracts
{
    public interface IOffreRepository
    {
        public Task<IEnumerable<offre>> Getoffres();
        public Task<offre> Getoffre(int offre_id);
        Task<IEnumerable<offre>> GetoffresByTypeOffre(string type_offre);
        public Task<offre> CreateOffre(OffreForCreationDto offre);
        public Task UpdateOffre(int offre_id, OffreForUpdateDto offre);
        public Task DeleteOffre(int offre_id);






        //public Task<IEnumerable<offre>> SearchOffreAsync(string search);
        Task<IEnumerable<offre>> SearchOffreAsync(string search);


    }

}
