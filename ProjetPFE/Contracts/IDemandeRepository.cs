using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Contracts
{
    public interface IDemandeRepository
    {


        public Task<IEnumerable<demande>> Getdemandes();
        public Task<demande> GetDemande(int demande_id);
        public Task<demande> CreateDemande(DemandeForCreationDto DemandeForCreationDto);
        public Task UpdateDemande(int demande_id, DemandeForUpdateDto demande);
        public Task DeleteDemande(int demande_id);
        Task<IEnumerable<demande>> SearchDemandeAsync(string search);
        List<demande> GetDemandesByResponsable(string matricule_resp);

        void ArchiverDemande(archive archive);



    }
}

