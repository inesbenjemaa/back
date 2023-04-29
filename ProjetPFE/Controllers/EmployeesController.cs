using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ProjetPFE.Context;
using ProjetPFE.Contracts;
using ProjetPFE.Contracts.services;
using ProjetPFE.Dto;
using ProjetPFE.Entities;
using ProjetPFE.Repository;

namespace ProjetPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase

    {


        private readonly IEmployeService employeService;
        private readonly IEmployeRepository _employeRepository;
        //private readonly DapperContext _context;

        public EmployeesController(IEmployeService employeService, IEmployeRepository employeRepository)
        {
            this.employeService = employeService;
            this._employeRepository = employeRepository;
        }



        [HttpGet]
        public async Task<ActionResult<ICollection<employe>>> RetrieveAllEmploye()
        {
            var employes = await this.employeService.RetrieveEmployes();
            return Ok(employes);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<employe>> GetEmployeByIdAsync(int id)
        {
            var employe = await _employeRepository.GetEmployeByIdAsync(id);

            if (employe == null)
            {
                return NotFound();
            }

            return employe;
        }





        //[HttpPost]
        //public async Task<IActionResult> AddEmploye([FromBody] employe employe)
        //{
        //    if (employe == null)
        //    {
        //        return BadRequest();
        //    }
        //    int rowsAffected = await _employeRepository.AddEmploye(employe);
        //    if (rowsAffected == 0)
        //    {
        //        return BadRequest("Unable to add employe.");
        //    }
        //    return Ok("Employe added successfully.");

        //}

        [HttpPost]
        public async Task<IActionResult> AddEmploye([FromBody] EmployeForCreationDto employe)
        {
            if (employe == null)
            {
                return BadRequest();
            }
            int rowsAffected = await _employeRepository.AddEmploye(employe);
            if (rowsAffected == 0)
            {
                return BadRequest("Unable to add employe.");
            }
            return Ok("Employe added successfully.");

        }






        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateEmploye(int id, employe employe)
        //{
        //    if (id != employe.employe_id)
        //    {
        //        return BadRequest("Employe ID mismatch.");
        //    }
        //    int rowsAffected = await _employeRepository.UpdateEmployeAsync(employe);
        //    if (rowsAffected == 0)
        //    {
        //        return BadRequest("Unable to update employe.");
        //    }
        //    return Ok("Employe updated successfully.");
        //}





        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmploye(int id, Empl employe)
        {
            if (id != employe.employe_id)
            {
                return BadRequest("Employe ID mismatch.");
            }
            int rowsAffected = await _employeRepository.UpdateEmployeAsync(employe);
            if (rowsAffected == 0)
            {
                return BadRequest("Unable to update employe.");
            }
            return Ok("Employe updated successfully.");
        }








        //[HttpDelete("{employe_id}")]
        //public async Task<IActionResult> DeleteEmploye(int employe_id)
        //{
        //    int rowsAffected = await _employeRepository.DeleteEmployeAsync(employe_id);
        //    if (rowsAffected == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok("Employe deleted successfully.");
        //}




        [HttpPost("login")]
        public async Task<ActionResult<employe>> Login(string email, string password)
        {
            var employe = await _employeRepository.Login(email, password);
            if (employe == null)
            {
                return BadRequest("Email or password is incorrect");
            }
            return employe;

        }




        //[HttpPost]
        //public async Task<IActionResult> CreateEmploye(EmployeForCreationDto employe)
        //{
        //    try
        //    {
        //        var createdemploye = await _employeRepository.CreateEmploye(employe);
        //        return CreatedAtRoute("employeById", new { employe_id = createdemploye.employe_id }, createdemploye);
        //    }
        //    catch (Exception ex)
        //    {
        //        //log error
        //        return StatusCode(500, ex.Message);
        //    }
        //}








        [HttpDelete("{employe_id}")]
        public async Task<IActionResult> DeleteEmploye(int employe_id)
        {
            try
            {
                var dbdemande = await _employeRepository.GetEmployeByIdAsync(employe_id);
                if (dbdemande == null)
                    return NotFound();

                await _employeRepository.DeleteEmploye(employe_id);
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
