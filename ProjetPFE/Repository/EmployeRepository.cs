using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Dapper.Contrib.Extensions;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using ProjetPFE.Dto;

namespace ProjetPFE.Repository
{
    public class EmployeRepository : IEmployeRepository
    {

        private readonly DapperContext _context;


        public EmployeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<ICollection<employe>> Getemployes()
        {
            var query = "SELECT employe.employe_id, employe.nom, employe.prenom, employe.matricule, employe.matricule_resp, employe.fonction, " +
            "employe.role, employe.date_recrutement, employe.email, employe.password, employe.compte_winds, diplome.diplome_id as diplome_id , " +
            "diplome.nom_diplome, diplome.lieu_diplome, diplome.employe_id, experience.experience_id as experience_id , experience.poste, " +
            "experience.entreprise, experience.date_debut, experience.date_fin, experience.employe_id, certification.certif_id as certif_id, " +
            "certification.nom_certif, certification.employe_id, technologie.techno_id as techno_id, technologie.nom_techno, technologie.employe_id  " +
            "FROM [dbo].[employe] as employe " +
            "LEFT JOIN [dbo].[diplome] as diplome ON employe.employe_id = diplome.employe_id " +
            "LEFT JOIN [dbo].[certification] as certification ON employe.employe_id = certification.employe_id " +
            "LEFT JOIN [dbo].[experience] as experience ON employe.employe_id = experience.employe_id " +
            "LEFT JOIN [dbo].[technologie] as technologie ON employe.employe_id = technologie.employe_id";


            var EmployeDictionary = new Dictionary<int, employe>();
            using (var _context = this._context.CreateConnection())
            {
                IEnumerable<employe> result = await _context.QueryAsync<employe, diplome, experience, certification, technologie, employe>(query, (emp, diplome, experience, certification, technologie) =>
                {
                    employe employe;
                    if (!EmployeDictionary.TryGetValue(emp.employe_id, out employe))
                    {
                        employe = emp;
                        employe.diplomes = new List<diplome>();
                        employe.experiences = new List<experience>();
                        employe.certifications = new List<certification>();
                        employe.technologies = new List<technologie>();
                        EmployeDictionary.Add(employe.employe_id, employe);
                    }
                    if (diplome != null) employe.diplomes.Add(diplome);
                    if (experience != null) employe.experiences.Add(experience);
                    if (certification != null) employe.certifications.Add(certification);
                    if (technologie != null) employe.technologies.Add(technologie);
                    return employe;
                }, splitOn: "diplome_id, experience_id, certif_id, techno_id");

                return result.ToList();
            }
        }



        public async Task<employe> GetEmployeByIdAsync(int id)
        {
            var query = @"SELECT employe.employe_id, employe.nom, employe.prenom, employe.matricule, employe.matricule_resp, 
                  employe.fonction, employe.role, employe.date_recrutement, employe.email, employe.password, employe.compte_winds, 
                  diplome.diplome_id as diplome_id , diplome.nom_diplome, diplome.lieu_diplome, diplome.employe_id, 
                  experience.experience_id as experience_id , experience.poste, experience.entreprise, experience.date_debut, 
                  experience.date_fin, experience.employe_id, certification.certif_id as certif_id, certification.nom_certif, 
                  certification.employe_id, technologie.techno_id as techno_id, technologie.nom_techno, technologie.employe_id  
                  FROM [dbo].[employe] as employe 
                  LEFT JOIN [dbo].[diplome] as diplome ON employe.employe_id = diplome.employe_id 
                  LEFT JOIN [dbo].[certification] as certification ON employe.employe_id = certification.employe_id 
                  LEFT JOIN [dbo].[experience] as experience ON employe.employe_id = experience.employe_id 
                  LEFT JOIN [dbo].[technologie] as technologie ON employe.employe_id = technologie.employe_id
                  WHERE employe.employe_id = @id";



            using (var _context = this._context.CreateConnection())
            {
                var employeDict = new Dictionary<int, employe>();
                var employeList = await _context.QueryAsync<employe, diplome, experience, certification, technologie, employe>(
                    query,
                    (emp, dip, exp, cert, tech) =>
                    {
                        if (!employeDict.TryGetValue(emp.employe_id, out employe employe))
                        {
                            employe = emp;
                            employe.diplomes = new List<diplome>();
                            employe.experiences = new List<experience>();
                            employe.certifications = new List<certification>();
                            employe.technologies = new List<technologie>();
                            employeDict.Add(employe.employe_id, employe);
                        }

                        if (dip != null && !employe.diplomes.Any(d => d.diplome_id == dip.diplome_id))
                        {
                            employe.diplomes.Add(dip);
                        }

                        if (exp != null && !employe.experiences.Any(e => e.experience_id == exp.experience_id))
                        {
                            employe.experiences.Add(exp);
                        }

                        if (cert != null && !employe.certifications.Any(c => c.certif_id == cert.certif_id))
                        {
                            employe.certifications.Add(cert);
                        }

                        if (tech != null && !employe.technologies.Any(t => t.techno_id == tech.techno_id))
                        {
                            employe.technologies.Add(tech);
                        }

                        return employe;
                    },
                    new { id },
                    splitOn: "diplome_id, experience_id, certif_id, techno_id");

                return employeList.FirstOrDefault();
            }
        }


        public string GenerateSha256Hash(string input)
        {
            /* create salt */
            //var rng = new RNGCryptoServiceProvider();
            //var buff = new byte[5];
            //rng.GetBytes(buff);
            //string salt = Convert.ToBase64String(buff);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            SHA256Managed shaString = new SHA256Managed();
            byte[] hash = shaString.ComputeHash(bytes);

            return BitConverter.ToString(hash);
        }


        //public async Task<int> AddEmploye(employe emp)
        //{
        //    var sql = @"INSERT INTO [dbo].[employe] (nom, prenom, matricule, matricule_resp, fonction, role, date_recrutement, email, password, compte_winds)
        //    VALUES (@nom, @prenom, @matricule, @matricule_resp, @fonction, @role, @date_recrutement, @email, @password, @compte_winds);
        //    SELECT CAST(SCOPE_IDENTITY() as int)";

        //    string hashedPassword = GenerateSha256Hash(emp.password);
        //    string password = hashedPassword.Replace("-", string.Empty).Substring(0, 16);

        //    using (var connection = _context.CreateConnection())
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                var employeId = await connection.QueryFirstOrDefaultAsync<int>(sql, new
        //                {
        //                    emp.nom,
        //                    emp.prenom,
        //                    emp.matricule,
        //                    emp.matricule_resp,
        //                    emp.fonction,
        //                    emp.role,
        //                    emp.date_recrutement,
        //                    emp.email,
        //                    password,
        //                    emp.compte_winds
        //                }, transaction);

        //                if (emp.diplomes != null)
        //                {
        //                    foreach (var diplome in emp.diplomes)
        //                    {
        //                        diplome.employe_id = employeId;
        //                        var sql_diplome = @"INSERT INTO [dbo].[diplome] (nom_diplome, lieu_diplome, employe_id)
        //                                    VALUES (@nom_diplome, @lieu_diplome, @employe_id);
        //                                    SELECT CAST(SCOPE_IDENTITY() as int)";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_diplome, new
        //                        {
        //                            diplome.nom_diplome,
        //                            diplome.lieu_diplome,
        //                            diplome.employe_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.experiences != null)
        //                {
        //                    foreach (var experience in emp.experiences)
        //                    {
        //                        experience.employe_id = employeId;
        //                        var sql_experience = @"INSERT INTO [dbo].[experience] (poste, date_debut, date_fin,entreprise, employe_id)
        //                                    VALUES (@poste, @date_debut, @date_fin,@entreprise, @employe_id);
        //                                    SELECT CAST(SCOPE_IDENTITY() as int)";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_experience, new
        //                        {
        //                            experience.poste,
        //                            experience.date_debut,
        //                            experience.date_fin,
        //                            experience.entreprise,
        //                            experience.employe_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.certifications != null)
        //                {
        //                    foreach (var certification in emp.certifications)
        //                    {
        //                        certification.employe_id = employeId;
        //                        var sql_certification = @"INSERT INTO [dbo].[certification] (nom_certif, employe_id)
        //                                    VALUES (@nom_certif, @employe_id);
        //                                    SELECT CAST(SCOPE_IDENTITY() as int)";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_certification, new
        //                        {
        //                            certification.nom_certif,
        //                            certification.employe_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.technologies != null)
        //                {
        //                    foreach (var technologie in emp.technologies)
        //                    {
        //                        technologie.employe_id = employeId;
        //                        var sql_technologie = @"INSERT INTO [dbo].[technologie] (nom_techno, employe_id)
        //                                    VALUES (@nom_techno, @employe_id);
        //                                    SELECT CAST(SCOPE_IDENTITY() as int)";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_technologie, new
        //                        {
        //                            technologie.nom_techno,
        //                            technologie.employe_id
        //                        }, transaction);
        //                    }
        //                }

        //                transaction.Commit();
        //                return employeId;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        //public async Task<int> UpdateEmployeAsync(employe emp)
        //{
        //    var existingEmploye = GetEmployeByIdAsync(emp.employe_id);
        //    if (existingEmploye == null)
        //        return 0;

        //    var sql = @"UPDATE employe " +
        //            "SET nom = @nom, prenom = @prenom, matricule = @matricule, matricule_resp = @matricule_resp, fonction = @fonction, " +
        //            "role = @role, date_recrutement = @date_recrutement, email = @email, password = @password, compte_winds = @compte_winds " +
        //            "WHERE employe_id = @employe_id";

        //    string hashedPassword = GenerateSha256Hash(emp.password);
        //    string password = hashedPassword.Replace("-", string.Empty).Substring(0, 16);

        //    using (var connection = _context.CreateConnection())
        //    {
        //        connection.Open();
        //        using (var transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                await connection.QueryFirstOrDefaultAsync<int>(sql, new
        //                {
        //                    emp.nom,
        //                    emp.prenom,
        //                    emp.matricule,
        //                    emp.matricule_resp,
        //                    emp.fonction,
        //                    emp.role,
        //                    emp.date_recrutement,
        //                    emp.email,
        //                    password,
        //                    emp.compte_winds,
        //                    emp.employe_id
        //                }, transaction);

        //                if (emp.diplomes != null)
        //                {
        //                    foreach (var diplome in emp.diplomes)
        //                    {
        //                        var sql_diplome = @"UPDATE diplome " +
        //                                           "SET nom_diplome = @nom_diplome, lieu_diplome = @lieu_diplome " +
        //                                           "WHERE diplome_id = @diplome_id";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_diplome, new
        //                        {
        //                            diplome.nom_diplome,
        //                            diplome.lieu_diplome,
        //                            diplome.diplome_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.experiences != null)
        //                {
        //                    foreach (var experience in emp.experiences)
        //                    {
        //                        var sql_experience = @"UPDATE experience " +
        //                                           "SET poste = @poste, date_debut = @date_debut, date_fin = @date_fin, entreprise = @entreprise " +
        //                                           "WHERE experience_id = @experience_id";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_experience, new
        //                        {
        //                            experience.poste,
        //                            experience.date_debut,
        //                            experience.date_fin,
        //                            experience.entreprise,
        //                            experience.experience_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.certifications != null)
        //                {
        //                    foreach (var certification in emp.certifications)
        //                    {
        //                        var sql_certification = @"UPDATE certification " +
        //                                           "SET nom_certif = @nom_certif " +
        //                                           "WHERE certif_id = @certif_id";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_certification, new
        //                        {
        //                            certification.nom_certif,
        //                            certification.certif_id
        //                        }, transaction);
        //                    }
        //                }

        //                if (emp.technologies != null)
        //                {
        //                    foreach (var technologie in emp.technologies)
        //                    {
        //                        var sql_technologie = @"UPDATE technologie " +
        //                                           "SET nom_techno = @nom_techno " +
        //                                           "WHERE techno_id = @techno_id";
        //                        await connection.QueryFirstOrDefaultAsync<int>(sql_technologie, new
        //                        {
        //                            technologie.nom_techno,
        //                            technologie.techno_id
        //                        }, transaction);
        //                    }
        //                }

        //                transaction.Commit();
        //                return emp.employe_id;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //}









        //public async Task<int> DeleteEmployeAsync(int id)
        //{
        //    var existingEmploye = GetEmployeByIdAsync(id);
        //    if (existingEmploye == null)
        //        return 0;

        //    string query = string.Empty;
        //    using (var _context = this._context.CreateConnection())
        //    {
        //        foreach (diplome dip in existingEmploye.Result.diplomes)
        //        {
        //            query = "DELETE FROM diplome WHERE diplome_id = @diplome_id";
        //            await _context.ExecuteAsync(query, new { diplome_id = dip.diplome_id });
        //        }
        //        foreach (experience exp in existingEmploye.Result.experiences)
        //        {
        //            query = "DELETE FROM experience WHERE experience_id = @experience_id";
        //            await _context.ExecuteAsync(query, new { experience_id = exp.experience_id });
        //        }
        //        foreach (certification cert in existingEmploye.Result.certifications)
        //        {
        //            query = "DELETE FROM certification WHERE certif_id = @certif_id";
        //            await _context.ExecuteAsync(query, new { certif_id = cert.certif_id });
        //        }
        //        foreach (technologie tech in existingEmploye.Result.technologies)
        //        {
        //            query = "DELETE FROM technologie WHERE techno_id = @techno_id";
        //            await _context.ExecuteAsync(query, new { techno_id = tech.techno_id });
        //        }

        //        query = "DELETE FROM employe WHERE employe_id = @employeId";
        //        int rowsAffected = await _context.ExecuteAsync(query, new { employeId = id });
        //        return 1;
        //    }
        //}






        public async Task<employe> GetEmployeByEmailAsync(string email)
        {
            var query = @"SELECT employe.employe_id, employe.nom, employe.prenom, employe.matricule, employe.matricule_resp, 
                  employe.fonction, employe.role, employe.date_recrutement, employe.email, employe.password, employe.compte_winds, 
                  diplome.diplome_id as diplome_id , diplome.nom_diplome, diplome.lieu_diplome, diplome.employe_id, 
                  experience.experience_id as experience_id , experience.poste, experience.entreprise, experience.date_debut, 
                  experience.date_fin, experience.employe_id, certification.certif_id as certif_id, certification.nom_certif, 
                  certification.employe_id, technologie.techno_id as techno_id, technologie.nom_techno, technologie.employe_id  
                  FROM [dbo].[employe] as employe 
                  LEFT JOIN [dbo].[diplome] as diplome ON employe.employe_id = diplome.employe_id 
                  LEFT JOIN [dbo].[certification] as certification ON employe.employe_id = certification.employe_id 
                  LEFT JOIN [dbo].[experience] as experience ON employe.employe_id = experience.employe_id 
                  LEFT JOIN [dbo].[technologie] as technologie ON employe.employe_id = technologie.employe_id
                  WHERE employe.email = @email";



            using (var _context = this._context.CreateConnection())
            {
                var employeDict = new Dictionary<int, employe>();
                var employeList = await _context.QueryAsync<employe, diplome, experience, certification, technologie, employe>(
                    query,
                    (emp, dip, exp, cert, tech) =>
                    {
                        if (!employeDict.TryGetValue(emp.employe_id, out employe employe))
                        {
                            employe = emp;
                            employe.diplomes = new List<diplome>();
                            employe.experiences = new List<experience>();
                            employe.certifications = new List<certification>();
                            employe.technologies = new List<technologie>();
                            employeDict.Add(employe.employe_id, employe);
                        }

                        if (dip != null && !employe.diplomes.Any(d => d.diplome_id == dip.diplome_id))
                        {
                            employe.diplomes.Add(dip);
                        }

                        if (exp != null && !employe.experiences.Any(e => e.experience_id == exp.experience_id))
                        {
                            employe.experiences.Add(exp);
                        }

                        if (cert != null && !employe.certifications.Any(c => c.certif_id == cert.certif_id))
                        {
                            employe.certifications.Add(cert);
                        }

                        if (tech != null && !employe.technologies.Any(t => t.techno_id == tech.techno_id))
                        {
                            employe.technologies.Add(tech);
                        }

                        return employe;
                    },
                    new { email },
                    splitOn: "diplome_id, experience_id, certif_id, techno_id");

                return employeList.FirstOrDefault();
            }
        }

        public async Task<employe?> Login(string email, string password)
        {
            string hashedPassword = GenerateSha256Hash(password);
            string protectPassword = hashedPassword.Replace("-", string.Empty).Substring(0, 16);

            var employe = await GetEmployeByEmailAsync(email);
            if (employe != null && employe.password == protectPassword)
                return employe;
            return null;

        }


        //public async Task<employe> CreateEmploye(EmployeForCreationDto employe)
        //{
        //    var query = "INSERT INTO employe (nom, prenom, matricule, matricule_resp, fonction, role, date_recrutement, email, password, compte_winds)  " +
        //        "VALUES (@nom, @prenom, @matricule, @matricule_resp, @fonction, @role, @date_recrutement, @email, @password, @compte_winds) SELECT CAST(SCOPE_IDENTITY() as int)";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("nom", employe.nom, DbType.String);
        //    parameters.Add("prenom", employe.prenom, DbType.String);
        //    parameters.Add("matricule", employe.matricule, DbType.String);
        //    parameters.Add("matricule_resp", employe.matricule_resp, DbType.String);
        //    parameters.Add("fonction", employe.fonction, DbType.String);
        //    parameters.Add("role", employe.role, DbType.String);
        //    parameters.Add("date_recrutement", employe.date_recrutement, DbType.DateTime);
        //    parameters.Add("email", employe.email, DbType.String);
        //    parameters.Add("password", employe.password, DbType.String);
        //    parameters.Add("compte_winds", employe.compte_winds, DbType.String);


        //    using (var connection = _context.CreateConnection())
        //    {
        //        var id = await connection.QuerySingleAsync<int>(query, parameters);

        //        var createdemploye = new employe
        //        {
        //            employe_id = id,
        //            nom = employe.nom,
        //            prenom = employe.prenom,
        //            matricule = employe.matricule,
        //            matricule_resp = employe.matricule_resp,
        //            fonction = employe.fonction,
        //            role = employe.role,
        //            date_recrutement = employe.date_recrutement,
        //            email = employe.email,
        //            password = employe.password,
        //            compte_winds = employe.compte_winds,


        //        };
        //        return createdemploye;
        //    }
        //}






        public async Task<int> AddEmploye(EmployeForCreationDto emp)
        {
            var sql = @"INSERT INTO [dbo].[employe] (nom, prenom, matricule, matricule_resp, fonction, role, date_recrutement, email, password, compte_winds)
            VALUES (@nom, @prenom, @matricule, @matricule_resp, @fonction, @role, @date_recrutement, @email, @password, @compte_winds);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            string hashedPassword = GenerateSha256Hash(emp.password);
            string password = hashedPassword.Replace("-", string.Empty).Substring(0, 16);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var employeId = await connection.QueryFirstOrDefaultAsync<int>(sql, new
                        {
                            emp.nom,
                            emp.prenom,
                            emp.matricule,
                            emp.matricule_resp,
                            emp.fonction,
                            emp.role,
                            emp.date_recrutement,
                            emp.email,
                            password,
                            emp.compte_winds
                        }, transaction);



                        transaction.Commit();
                        return employeId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }









        public async Task<int> UpdateEmployeAsync(Empl emp)
        {
            var existingEmploye = GetEmployeByIdAsync(emp.employe_id);
            if (existingEmploye == null)
                return 0;

            var sql = @"UPDATE employe " +
                    "SET nom = @nom, prenom = @prenom, matricule = @matricule, matricule_resp = @matricule_resp, fonction = @fonction, " +
                    "role = @role, date_recrutement = @date_recrutement, email = @email, password = @password, compte_winds = @compte_winds " +
                    "WHERE employe_id = @employe_id";

            string hashedPassword = GenerateSha256Hash(emp.password);
            string password = hashedPassword.Replace("-", string.Empty).Substring(0, 16);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.QueryFirstOrDefaultAsync<int>(sql, new
                        {
                            emp.nom,
                            emp.prenom,
                            emp.matricule,
                            emp.matricule_resp,
                            emp.fonction,
                            emp.role,
                            emp.date_recrutement,
                            emp.email,
                            password,
                            emp.compte_winds,
                            emp.employe_id
                        }, transaction);

                       
                        transaction.Commit();
                        return emp.employe_id;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

        }








        public async Task DeleteEmploye(int employe_id)
        {
            var query = "DELETE FROM employe WHERE employe_id = @employe_id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { employe_id });
            }
        }














    }
}
