using Dapper;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using System.Data;

namespace ProjetPFE.Repository
{
    
        public class ArchiveRepository : IArchiveRepository
        {
            private readonly DapperContext _context;

            public ArchiveRepository(DapperContext context)
            {
                _context = context;
            }



            public async Task<IEnumerable<archive>> GetArchives()
            {
                var query = "SELECT * FROM archive";

                using (var connection = _context.CreateConnection())
                {
                    var archives = await connection.QueryAsync<archive>(query);
                    return archives.ToList();
                }
            }



            public async Task<archive> GetArchive(int archive_id)
            {
                var query = "SELECT * FROM archive WHERE archive_id = @archive_id";

                using (var connection = _context.CreateConnection())
                {
                    var archive = await connection.QuerySingleOrDefaultAsync<archive>(query, new { archive_id });

                    return archive;
                }
            }




            public async Task<archive> CreateArchive(ArchiveForCreationDto archive)
            {
                var query = "INSERT INTO archive (  demande_id, nb_a_exp, type_demande, titre_fonction, nature_contrat, lien_fichier, " +
                    "nom_fichier, remarque,  collaborateur_remp, " +
                    "date_creation, affectation ) VALUES (@demande_id, @nb_a_exp, @type_demande, @titre_fonction, @nature_contrat, " +
                    "@lien_fichier, @nom_fichier, @remarque, " +
                    " @collaborateur_remp, @date_creation, @affectation)" + "SELECT CAST(SCOPE_IDENTITY() as int)";

                var parameters = new DynamicParameters();
                parameters.Add("demande_id", archive.demande_id, DbType.Int32);
                parameters.Add("nb_a_exp", archive.nb_a_exp, DbType.Int32);
                parameters.Add("type_demande", archive.type_demande, DbType.String);
                parameters.Add("titre_fonction", archive.titre_fonction, DbType.String);
                parameters.Add("nature_contrat", archive.nature_contrat, DbType.String);
                parameters.Add("lien_fichier", archive.lien_fichier, DbType.String);
                parameters.Add("nom_fichier", archive.nom_fichier, DbType.String);
                parameters.Add("remarque", archive.remarque, DbType.String);
                parameters.Add("collaborateur_remp", archive.collaborateur_remp, DbType.String);
                parameters.Add("date_creation", archive.date_creation, DbType.DateTime);
                parameters.Add("affectation", archive.affectation, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    var id = await connection.QuerySingleAsync<int>(query, parameters);

                    var createdarchive = new archive
                    {
                        archive_id = id,
                        demande_id = archive.demande_id,
                        nb_a_exp = archive.nb_a_exp,
                        type_demande = archive.type_demande,
                        titre_fonction = archive.titre_fonction,
                        lien_fichier = archive.lien_fichier,
                        nom_fichier = archive.nom_fichier,
                        remarque = archive.remarque,
                        nature_contrat = archive.nature_contrat,
                        collaborateur_remp = archive.collaborateur_remp,
                        date_creation = archive.date_creation,
                        affectation = archive.affectation,

                    };
                    return createdarchive;
                }
            }


            public async Task UpdateArchive(int archive_id, ArchiveForUpdateDto archive)
            {
                var query = "UPDATE archive SET demande_id = @demande_id,  nb_a_exp = @nb_a_exp, type_demande = @type_demande, titre_fonction = @titre_fonction, " +
                    "lien_fichier = @lien_fichier, nom_fichier = @nom_fichier, " +
                    "  remarque = @remarque," +
                    " nature_contrat = @nature_contrat, " +
                    " collaborateur_remp = @collaborateur_remp, " +
                    "date_creation = @date_creation, affectation = @affectation WHERE archive_id = @archive_id";

                var parameters = new DynamicParameters();
            parameters.Add("demande_id", archive.demande_id, DbType.Int32);
            parameters.Add("archive_id", archive_id, DbType.Int32);
            parameters.Add("nb_a_exp", archive.nb_a_exp, DbType.Int32);
            parameters.Add("type_demande", archive.type_demande, DbType.String);
            parameters.Add("titre_fonction", archive.titre_fonction, DbType.String);
            parameters.Add("nature_contrat", archive.nature_contrat, DbType.String);
            parameters.Add("lien_fichier", archive.lien_fichier, DbType.String);
            parameters.Add("nom_fichier", archive.nom_fichier, DbType.String);
            parameters.Add("remarque", archive.remarque, DbType.String);
            parameters.Add("collaborateur_remp", archive.collaborateur_remp, DbType.String);
            parameters.Add("date_creation", archive.date_creation, DbType.DateTime);
            parameters.Add("affectation", archive.affectation, DbType.String);

            using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }



            public async Task DeleteArchive(int archive_id)
            {
                var query = "DELETE FROM archive WHERE archive_id = @archive_id";

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { archive_id });
                }
            }




        }
    
}
