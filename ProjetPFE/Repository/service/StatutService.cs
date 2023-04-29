using ProjetPFE.Contracts.services;
using ProjetPFE.Helpers;

namespace ProjetPFE.Repository.service
{
    public class StatutService : IStatutService 
    {
        public statut GetStatutDemande(statut statut_chef, statut statut_rh, statut statut_ds)
        {
            statut statut_demande;

            if (statut_chef == statut.en_attente)
            {
                statut_demande = statut.en_cours;
                statut_rh = statut.en_cours;
                statut_ds = statut.en_cours;
            }
            else if (statut_chef == statut.valide)
            {
                if (statut_rh == statut.valide)
                {
                    if (statut_ds == statut.valide)
                    {
                        statut_demande = statut.valide;
                    }
                    else if (statut_ds == statut.refuse)
                    {
                        statut_demande = statut.refuse;
                    }
                    else
                    {
                        statut_demande = statut.en_attente;
                        statut_ds = statut.en_attente;
                    }
                }
                else if (statut_rh == statut.refuse)
                {
                    statut_demande = statut.refuse;
                    statut_ds = statut.en_cours;
                }
                
                else
                {
                    statut_demande = statut.en_attente;
                    statut_rh = statut.en_attente;
                    statut_ds = statut.en_cours;
                }

            }
            else if (statut_chef == statut.en_cours && statut_rh == statut.en_cours && statut_ds == statut.en_cours)
            {
                statut_demande = statut.en_cours;
            }
            else // statut_chef == statut.refuse
            {
                statut_demande = statut.refuse;
                statut_rh = statut.en_cours;
                statut_ds = statut.en_cours;
            }
            

            return statut_demande;
        }

    }
}
