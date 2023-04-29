using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using AutoMapper;
using ProjetPFE.Contracts.services;
using ProjetPFE.Helpers;
using ProjetPFE.Repository;
using ProjetPFE.EmailService.Interfaces;
using ProjetPFE.EmailService;
using ProjetPFE.EmailService.EmailEntities;

namespace ProjetPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class DemandesController : ControllerBase
    {
        private readonly IDemandeRepository _demandeRepo;
        private readonly IMapper _mapper;
        private readonly IStatutService _statutService;
       // private readonly IArchiveRepository _archiveRepo;





        public DemandesController(IDemandeRepository demandeRepo, IMapper mapper, IStatutService statutService)
        {
            _demandeRepo = demandeRepo;
            _mapper = mapper;
            _statutService = statutService;
            

        }


        [HttpGet]
        public async Task<IActionResult> GetDemandes()
        {
            try
            {
                
                var demandes = await _demandeRepo.Getdemandes();
                return Ok(demandes);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{demande_id}", Name = "demandeById")]
        public async Task<IActionResult> GetDemande(int demande_id)
        {
            try
            {
                var demande = await _demandeRepo.GetDemande(demande_id);
                if (demande == null)
                    return NotFound();

                return Ok(demande);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        
        [HttpPost]
        public async Task<IActionResult> CreateDemande([FromForm] DemandeForCreationDto DemandeForCreationDto)
        {
            try
            {
                // Vérifier si les données de la demande sont valides
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Mapper la demande à partir du DTO
                var nouvelleDemande = _mapper.Map<DemandeForCreationDto>(DemandeForCreationDto);

                // Enregistrer la demande dans la base de données
                var createdDemande = await _demandeRepo.CreateDemande(nouvelleDemande);

                return CreatedAtRoute("demandeById", new { demande_id = createdDemande.demande_id }, createdDemande);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }








        [HttpPut("{demande_id}")]
        public async Task<IActionResult> UpdateDemande(int demande_id, DemandeForUpdateDto demande)
        {
            try
            {
                var dbdemande = await _demandeRepo.GetDemande(demande_id);
                if (dbdemande == null)
                    return NotFound();

                await _demandeRepo.UpdateDemande(demande_id, demande);
                return Ok("Demande updated successfully.");
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete("{demande_id}")]
        public async Task<IActionResult> DeleteDemande(int demande_id)
        {
            try
            {
                var dbdemande = await _demandeRepo.GetDemande(demande_id);
                if (dbdemande == null)
                    return NotFound();

                await _demandeRepo.DeleteDemande(demande_id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }





        [Route("SearchDemande/{search}")]
        [HttpGet]
        public async Task<IEnumerable<demande>?> SearchDemande(string search)
        {
            if (search == null || string.IsNullOrEmpty(search))
            {
                return null;
            }

            var result = await _demandeRepo.SearchDemandeAsync(search);

            return result;
        }





        [HttpGet("{statut_chef}/{statut_rh}/{statut_ds}")]
        public IActionResult GetStatutDemande(string statut_chef, string statut_rh, string statut_ds)
        {
            if (!Enum.TryParse<statut>(statut_chef, out var statutChef) ||
                !Enum.TryParse<statut>(statut_rh, out var statutRh) ||
                !Enum.TryParse<statut>(statut_ds, out var statutDs))
            {
                return BadRequest("Les statuts spécifiés sont invalides.");
            }

            var statutDemande = _statutService.GetStatutDemande(statutChef, statutRh, statutDs);

            return Ok(statutDemande.ToString());
        }





        [HttpGet("responsable/{matricule_resp}")]
        public IActionResult GetDemandesByResponsable(string matricule_resp)
        {
            try
            {
                var demandes = _demandeRepo.GetDemandesByResponsable(matricule_resp);
                return Ok(demandes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        //[HttpPost]
        //public async Task<IActionResult> ArchiverDemande(int demande_id)
        //{
        //    var demande = await _demandeRepo.GetDemande(demande_id);

        //    if (demande == null)
        //    {
        //        return NotFound();
        //    }

        //    // Si la demande a été validée par le DS
        //    if (demande.statut_ds == statut.valide.ToString())
        //    {
        //        var archive = new archive
        //        {
        //            demande_id = demande.demande_id,
        //            nb_a_exp = demande.nb_a_exp,
        //            type_demande = demande.type_demande,
        //            titre_fonction = demande.titre_fonction,
        //            nature_contrat = demande.nature_contrat,
        //            lien_fichier = demande.lien_fichier,
        //            nom_fichier = demande.nom_fichier,
        //            remarque = demande.remarque,
        //            collaborateur_remp = demande.collaborateur_remp,
        //            date_creation = demande.date_creation,
        //            affectation = demande.affectation
        //        };

        //        _demandeRepo.ArchiverDemande(archive);

        //        return RedirectToAction("Index", "Home");
        //    }
        //    // Si la demande a été refusée par le chef, le RH ou le DS
        //    else if (demande.statut_chef == statut.refuse.ToString() ||
        //             demande.statut_rh == statut.refuse.ToString() ||
        //             demande.statut_ds == statut.refuse.ToString())
        //    {
        //        var archive = new archive
        //        {
        //            demande_id = demande.demande_id,
        //            nb_a_exp = demande.nb_a_exp,
        //            type_demande = demande.type_demande,
        //            titre_fonction = demande.titre_fonction,
        //            nature_contrat = demande.nature_contrat,
        //            lien_fichier = demande.lien_fichier,
        //            nom_fichier = demande.nom_fichier,
        //            remarque = demande.remarque,
        //            collaborateur_remp = demande.collaborateur_remp,
        //            date_creation = demande.date_creation,
        //            affectation = demande.affectation
        //        };

        //        //if (demande.statut_chef == statut.refuse.ToString())
        //        //{
        //        //    archive.motif_chef = demande.motif_chef;
        //        //}

        //        //if (demande.statut_rh == statut.refuse.ToString())
        //        //{
        //        //    archive.motif_rh = demande.motif_rh;
        //        //}

        //        //if (demande.statut_ds == statut.refuse.ToString())
        //        //{
        //        //    archive.motif_ds = demande.motif_ds;
        //        //}

        //        _demandeRepo.ArchiverDemande(archive);

        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}



    }
}


