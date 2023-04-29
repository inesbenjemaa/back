using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Contracts;
using ProjetPFE.Dto;

namespace ProjetPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ArchiveController : ControllerBase
    {
        private readonly IArchiveRepository _archiveRepo;
        public ArchiveController(IArchiveRepository archiveRepo)
        {
            _archiveRepo = archiveRepo;
        }



        [HttpGet]
        public async Task<IActionResult> GetArchives()
        {
            try
            {
                var Archives = await _archiveRepo.GetArchives();
                return Ok(Archives);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }




        [HttpGet("{archive_id}", Name = "archiveById")]
        public async Task<IActionResult> GetArchive(int archive_id)
        {
            try
            {
                var archive = await _archiveRepo.GetArchive(archive_id);
                if (archive == null)
                    return NotFound();

                return Ok(archive);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateArchive(ArchiveForCreationDto archive)
        {
            try
            {
                var createdarchive = await _archiveRepo.CreateArchive(archive);
                return CreatedAtRoute("archiveById", new { archive_id = createdarchive.archive_id }, createdarchive);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{archive_id}")]
        public async Task<IActionResult> UpdateArchive(int archive_id, ArchiveForUpdateDto archive)
        {
            try
            {
                var dbdemande = await _archiveRepo.GetArchive(archive_id);
                if (dbdemande == null)
                    return NotFound();

                await _archiveRepo.UpdateArchive(archive_id, archive);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete("{archive_id}")]
        public async Task<IActionResult> DeleteArchive(int archive_id)
        {
            try
            {
                var dbarchive = await _archiveRepo.GetArchive(archive_id);
                if (dbarchive == null)
                    return NotFound();

                await _archiveRepo.DeleteArchive(archive_id);
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
