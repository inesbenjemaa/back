using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiplomeController : ControllerBase
    {

            private readonly IDiplomeRepository _DiplomeRepo;
            public DiplomeController(IDiplomeRepository DiplomeRepo)
            {
               _DiplomeRepo = DiplomeRepo;
            }




            [HttpGet]
            public async Task<IActionResult> GetDiplomes()
            {
                try
                {
                    var dip = await _DiplomeRepo.GetDiplomes();
                    return Ok(dip);
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(500, ex.Message);
                }
            }



            [HttpGet("{diplome_id}", Name = "diplomebyid")]
            public async Task<IActionResult> GetDiplome(int diplome_id)

            {
                try
                {
                    var diplome = await _DiplomeRepo.GetDiplome(diplome_id);
                    if (diplome == null)
                        return NotFound();

                    return Ok(diplome);
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(500, ex.Message);
                }
            }







            [HttpPost]
            public async Task<IActionResult> CreateDiplome(DiplomeForCreationDto diplome)
            {
                try
                {
                    var createddip = await _DiplomeRepo.CreateDiplome(diplome);
                    return CreatedAtRoute("diplomeById", new { diplome_id = createddip.diplome_id }, createddip);
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(500, ex.Message);
                }
            }





            [HttpPut("{diplome_id}")]
            public async Task<IActionResult> UpdateDiplome(int diplome_id, DiplomeForUpdateDto diplome)
            {
                try
                {
                    var dbdip = await _DiplomeRepo.GetDiplome(diplome_id);
                    if (dbdip == null)
                        return NotFound();

                    await _DiplomeRepo.UpdateDiplome(diplome_id, diplome);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(500, ex.Message);
                }
            }





            [HttpDelete("{diplome_id}")]
            public async Task<IActionResult> DeleteOffre(int diplome_id)
            {
                try
                {
                    var dbdip = await _DiplomeRepo.GetDiplome(diplome_id);
                    if (dbdip == null)
                        return NotFound();

                    await _DiplomeRepo.GetDiplome(diplome_id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    //log error
                    return StatusCode(500, ex.Message);
                }
            }






        





        

    }
}
