using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using ProjetPFE.Repository;

namespace ProjetPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffreController : ControllerBase
    {
        private readonly IOffreRepository _offreRepo;
        public OffreController(IOffreRepository offreRepo)
        {
            _offreRepo = offreRepo;
        }




        [HttpGet]
        public async Task<IActionResult> Getoffres()
        {
            try
            {
                var offres = await _offreRepo.Getoffres();
                return Ok(offres);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{offre_id}", Name = "offreById")]
        public async Task<IActionResult> Getoffre(int offre_id)

        {
            try
            {
                var offre = await _offreRepo.Getoffre(offre_id);
                if (offre == null)
                    return NotFound();

                return Ok(offre);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("type/{type_offre}")]
        public async Task<IActionResult> GetOffresByTypeOffre(string type_offre)
        {
            var offres = await _offreRepo.GetoffresByTypeOffre(type_offre);

            if (offres == null)
            {
                return NotFound();
            }

            return Ok(offres);
        }



        [HttpPost]
        public async Task<IActionResult> CreateOffre(OffreForCreationDto offre)
        {
            try
            {
                var createdoffre = await _offreRepo.CreateOffre(offre);
                return CreatedAtRoute("offreById", new { offre_id = createdoffre.offre_id }, createdoffre);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }





        [HttpPut("{offre_id}")]
        public async Task<IActionResult> UpdateOffre(int offre_id, OffreForUpdateDto offre)
        {
            try
            {
                var dbdemande = await _offreRepo.Getoffre(offre_id);
                if (dbdemande == null)
                    return NotFound();

                await _offreRepo.UpdateOffre(offre_id, offre);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }





        [HttpDelete("{offre_id}")]
        public async Task<IActionResult> DeleteOffre(int offre_id)
        {
            try
            {
                var dbdemande = await _offreRepo.Getoffre(offre_id);
                if (dbdemande == null)
                    return NotFound();

                await _offreRepo.DeleteOffre(offre_id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }






        //[Route("SearchOffre/{Search}")]
        //[HttpGet]
        //public async Task<IEnumerable<offre>> GetOffre(string search)
        //{
        //    if (search == null || string.IsNullOrEmpty(search))
        //    {
        //        return null;
        //    }
        //    return await _repo.SearchOffreAsync(search)
        //}


        [Route("SearchOffre/{search}")]
        [HttpGet]
        public async Task<IEnumerable<offre>?> SearchOffre(string search)
        {
            if (search == null || string.IsNullOrEmpty(search))
            {
                return null;
            }

            var result = await _offreRepo.SearchOffreAsync(search);

            return result;
        }







    }



}

