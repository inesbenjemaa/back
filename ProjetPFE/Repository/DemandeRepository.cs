using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using ProjetPFE.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace ProjetPFE.Repository
{
    public class DemandeRepository : IDemandeRepository
    {
        private readonly DapperContext _context;

        public DemandeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<demande>> Getdemandes()
        {
            var query = "SELECT * FROM demande";

            using (var connection = _context.CreateConnection())
            {
                var demandes = await connection.QueryAsync<demande>(query);
                return demandes.ToList();
            }
        }



        public async Task<demande> GetDemande(int demande_id)
        {
            var query = "SELECT * FROM demande WHERE demande_id = @demande_id";

            using (var connection = _context.CreateConnection())
            {
                var demande = await connection.QuerySingleOrDefaultAsync<demande>(query, new { demande_id });

                return demande;
            }
        }




        public async Task<demande> CreateDemande(DemandeForCreationDto DemandeForCreationDto)
        {
            var query = "INSERT INTO demande ( offre_id, employe_id,  nb_a_exp, type_demande, titre_fonction, nature_contrat, lien_fichier, " +
                "nom_fichier, remarque, statut_chef, statut_rh, statut_ds, motif_chef, motif_rh, motif_ds, collaborateur_remp, " +
                "date_creation, affectation ) VALUES (@offre_id, @employe_id,  @nb_a_exp, @type_demande, @titre_fonction, @nature_contrat, " +
                "@lien_fichier, @nom_fichier, @remarque, @statut_chef, @statut_rh, @statut_ds, @motif_chef, @motif_rh, @motif_ds," +
                " @collaborateur_remp, @date_creation, @affectation)" + "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("offre_id", DemandeForCreationDto.offre_id, DbType.Int32);
            parameters.Add("employe_id", DemandeForCreationDto.employe_id, DbType.Int32);
            parameters.Add("nb_a_exp", DemandeForCreationDto.nb_a_exp, DbType.Int32);
            parameters.Add("type_demande", DemandeForCreationDto.type_demande, DbType.String);
            parameters.Add("titre_fonction", DemandeForCreationDto.titre_fonction, DbType.String);
            parameters.Add("nature_contrat", DemandeForCreationDto.nature_contrat, DbType.String);
            parameters.Add("remarque", DemandeForCreationDto.remarque, DbType.String);
            parameters.Add("statut_chef", DemandeForCreationDto.statut_chef, DbType.String);
            parameters.Add("statut_rh", DemandeForCreationDto.statut_rh, DbType.String);
            parameters.Add("statut_ds", DemandeForCreationDto.statut_ds, DbType.String);
            parameters.Add("motif_chef", DemandeForCreationDto.motif_chef, DbType.String);
            parameters.Add("motif_rh", DemandeForCreationDto.motif_rh, DbType.String);
            parameters.Add("motif_ds", DemandeForCreationDto.motif_ds, DbType.String);
            parameters.Add("collaborateur_remp", DemandeForCreationDto.collaborateur_remp, DbType.String);
            parameters.Add("date_creation", DemandeForCreationDto.date_creation, DbType.DateTime);
            parameters.Add("affectation", DemandeForCreationDto.affectation, DbType.String);

            if (DemandeForCreationDto.File != null && DemandeForCreationDto.File.Length > 0)
            {
                // Générer un nom de fichier unique
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(DemandeForCreationDto.File.FileName);

                // Déterminer le chemin où enregistrer le fichier
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

                // Copier le fichier vers le serveur
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await DemandeForCreationDto.File.CopyToAsync(stream);
                }

                parameters.Add("lien_fichier", filePath, DbType.String);
                parameters.Add("nom_fichier", fileName, DbType.String);
                }
            else
               {
               parameters.Add("lien_fichier", string.Empty, DbType.String);
               parameters.Add("nom_fichier", string.Empty, DbType.String);
               }
            
            
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createddemande = new demande
                {
                    demande_id = id,
                    offre_id = DemandeForCreationDto.offre_id,
                    employe_id = DemandeForCreationDto.employe_id,
                    nb_a_exp = DemandeForCreationDto.nb_a_exp,
                    type_demande = DemandeForCreationDto.type_demande,
                    titre_fonction = DemandeForCreationDto.titre_fonction,
                    statut_chef = DemandeForCreationDto.statut_chef,
                    statut_rh = DemandeForCreationDto.statut_rh,
                    statut_ds = DemandeForCreationDto.statut_ds,
                    remarque = DemandeForCreationDto.remarque,
                    nature_contrat = DemandeForCreationDto.nature_contrat,
                    motif_chef = DemandeForCreationDto.motif_chef,
                    motif_rh = DemandeForCreationDto.motif_rh,
                    motif_ds = DemandeForCreationDto.motif_ds,
                    collaborateur_remp = DemandeForCreationDto.collaborateur_remp,
                    date_creation = DemandeForCreationDto.date_creation,
                    affectation = DemandeForCreationDto.affectation,

                };
                return createddemande;
            }
        }
    

        public async Task UpdateDemande(int demande_id, DemandeForUpdateDto demande)
    {
        var query = "UPDATE demande SET offre_id = @offre_id, employe_id = @employe_id,  nb_a_exp = @nb_a_exp, type_demande = @type_demande, titre_fonction = @titre_fonction, " +
            "lien_fichier = @lien_fichier, nom_fichier = @nom_fichier, statut_chef = @statut_chef, statut_rh = @statut_rh," +
            " statut_ds = @statut_ds, remarque = @remarque," +
            " nature_contrat = @nature_contrat, motif_chef = @motif_chef, motif_rh = @motif_rh, motif_ds = @motif_ds," +
            " collaborateur_remp = @collaborateur_remp, " +
            "date_creation = @date_creation, affectation = @affectation WHERE demande_id = @demande_id";

        var parameters = new DynamicParameters();
        parameters.Add("offre_id", demande.offre_id, DbType.Int32);
        parameters.Add("employe_id", demande.employe_id, DbType.Int32);
        parameters.Add("nb_a_exp", demande.nb_a_exp, DbType.Int32);
        parameters.Add("type_demande", demande.type_demande, DbType.String);
        parameters.Add("titre_fonction", demande.titre_fonction, DbType.String);
        parameters.Add("nature_contrat", demande.nature_contrat, DbType.String);
        parameters.Add("lien_fichier", demande.lien_fichier, DbType.String);
        parameters.Add("nom_fichier", demande.nom_fichier, DbType.String);
        parameters.Add("remarque", demande.remarque, DbType.String);
        parameters.Add("statut_chef", demande.statut_chef, DbType.String);
        parameters.Add("statut_rh", demande.statut_rh, DbType.String);
        parameters.Add("statut_ds", demande.statut_ds, DbType.String);
        parameters.Add("motif_chef", demande.motif_chef, DbType.String);
        parameters.Add("motif_rh", demande.motif_rh, DbType.String);
        parameters.Add("motif_ds", demande.motif_ds, DbType.String);
        parameters.Add("collaborateur_remp", demande.collaborateur_remp, DbType.String);
        parameters.Add("date_creation", demande.date_creation, DbType.DateTime);
        parameters.Add("affectation", demande.affectation, DbType.String);
        parameters.Add("demande_id", demande_id, DbType.Int32);


        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters);
        }
    }



       public async Task DeleteDemande(int demande_id)
    {
        var query = "DELETE FROM demande WHERE demande_id = @demande_id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { demande_id });
        }
    }





        public async Task<IEnumerable<demande>> SearchDemandeAsync(string search)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                   SELECT demande.*, employe.fonction
                   FROM demande
                   INNER JOIN employe ON demande.employe_id = employe.employe_id
                   WHERE LOWER(type_demande) LIKE '%' + @search + '%'
                   OR LOWER(titre_fonction) LIKE '%' + @search + '%'
                   OR LOWER(nature_contrat) LIKE '%' + @search + '%'
                   OR LOWER(employe.fonction) LIKE '%' + @search + '%'
                   ORDER BY demande.demande_id DESC;
                   ";

                var result = await connection.QueryAsync<demande>(
                    sql,
                    new { search = search.ToLower() });

                return result;
            }
        }





        public List<demande> GetDemandesByResponsable(string matricule_resp)
        {
            using (var connection = _context.CreateConnection())
            {
                // Récupérer l'employé connecté en fonction de son matricule
                employe responsable = connection.QueryFirstOrDefault<employe>("SELECT * FROM employe WHERE matricule = @matricule", new { matricule = matricule_resp });

                if (responsable == null)
                {
                    throw new Exception("Employé non trouvé !");
                }

                // Récupérer les demandes des employés dont le responsable est l'utilisateur connecté
                string query = "SELECT * FROM demande WHERE demande_id  IN (SELECT demande_id FROM employe WHERE matricule_resp = @matricule_resp)";
                List<demande> demandes = connection.Query<demande>(query, new { matricule_resp = responsable.matricule }).ToList();

                return demandes;
            }
        }






        public void ArchiverDemande(archive archive)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insérer la demande dans la table Archive
                        var archiveQuery = @"INSERT INTO archive (demande_id, nb_a_exp, type_demande, titre_fonction, nature_contrat, lien_fichier, nom_fichier, remarque, collaborateur_remp, date_creation, affectation, motif_chef, motif_rh, motif_ds)
                                     VALUES (@demande_id, @nb_a_exp, @type_demande, @titre_fonction, @nature_contrat, @lien_fichier, @nom_fichier, @remarque, @collaborateur_remp, @date_creation, @affectation, @motif_chef, @motif_rh, @motif_ds)";

                        connection.Execute(archiveQuery, archive, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }



    }
}
