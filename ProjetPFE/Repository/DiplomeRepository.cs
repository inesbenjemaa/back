using Dapper;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using System.Data;

namespace ProjetPFE.Repository
{
   



        public class DiplomeRepository : IDiplomeRepository
        {
            private readonly DapperContext _context;

            public DiplomeRepository(DapperContext context)
            {
                _context = context;
            }




            public async Task<IEnumerable<diplome>> GetDiplomes()
            {
                var query = "SELECT * FROM diplome";

                using (var connection = _context.CreateConnection())
                {
                    var diplomes = await connection.QueryAsync<diplome>(query);
                    return diplomes.ToList();
                }
            }




            public async Task<diplome> GetDiplome(int diplome_id)
            {
                var query = "SELECT * FROM diplome WHERE diplome_id = @diplome_id";

                using (var connection = _context.CreateConnection())
                {
                    var dip = await connection.QuerySingleOrDefaultAsync<diplome>(query, new { diplome_id });

                    return dip;
                }
            }




   


        public async Task<diplome> CreateDiplome(DiplomeForCreationDto diplome)
            {
                var query = "INSERT INTO diplome (employe_id, nom_diplome, lieu_diplome) " +
                    "VALUES ( @employe_id, @nom_diplome, @lieu_diplome) SELECT CAST(SCOPE_IDENTITY() as int)";

                var parameters = new DynamicParameters();
                parameters.Add("employe_id", diplome.employe_id, DbType.Int32);
                parameters.Add("nom_diplome", diplome.nom_diplome, DbType.String);
                parameters.Add("lieu_diplome", diplome.lieu_diplome, DbType.String);
               


                using (var connection = _context.CreateConnection())
                {
                    var id = await connection.QuerySingleAsync<int>(query, parameters);

                    var createddip = new diplome
                    {
                        diplome_id = id,
                        employe_id = diplome.employe_id,
                        nom_diplome = diplome.nom_diplome,
                        lieu_diplome = diplome.lieu_diplome,
                        

                    };
                    return createddip;
                }
            }



            public async Task UpdateDiplome(int diplome_id, DiplomeForUpdateDto diplome)
            {
                var query = "UPDATE diplome SET  nom_diplome = @nom_diplome, lieu_diplome = @lieu_diplome, employe_id = @employe_id " +
                    " WHERE diplome_id = @diplome_id";

                var parameters = new DynamicParameters();
                parameters.Add("diplome_id", diplome_id, DbType.Int32);
                parameters.Add("employe_id", diplome.employe_id, DbType.Int32);
                parameters.Add("nom_diplome", diplome.nom_diplome, DbType.String);
                parameters.Add("lieu_diplome", diplome.lieu_diplome, DbType.String);
               

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }




            public async Task DeleteDiplome(int diplome_id)
            {
                var query = "DELETE FROM diplome WHERE diplome_id = @diplome_id";

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { diplome_id });
                }
            }



         





           



        }
    
}
