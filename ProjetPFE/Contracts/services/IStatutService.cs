using ProjetPFE.Helpers;

namespace ProjetPFE.Contracts.services
{
    public interface IStatutService
    {

        statut GetStatutDemande(statut statut_chef, statut statut_rh, statut statut_ds);

    }
}
