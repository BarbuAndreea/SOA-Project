using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDent.Domain.DTO;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using MyDent.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MyDent.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {

        private readonly IClinicService _clinicService;
        private readonly IUserService _userService;
        public SuperAdminController(IClinicService clinicService,IUserService userService)
        {
            _clinicService = clinicService;
            _userService = userService;
        }

        [Authorize(UserRole.SuperAdmin)]
        [HttpPost("add_clinic")]
        public ActionResult<Clinic> AddNewClinic([FromBody] ClinicDto newClinic)
        {
            return _clinicService.AddClinic(newClinic);
        }

        [Authorize(UserRole.SuperAdmin)]
        [HttpPut("update_clinic")]
        public ActionResult<Clinic> UpdateClinic([FromBody] ClinicDto newClinic)
        {
            return _clinicService.UpdateClinic(newClinic);
        }

        [Authorize(UserRole.Patient, UserRole.SuperAdmin, UserRole.ClinicAdmin)]
        [HttpGet("get-clinics")]
        public ActionResult<List<Clinic>> GetClinics()
        {
            return _clinicService.GetAllClinics().ToList();
        }

        [Authorize(UserRole.SuperAdmin)]
        [HttpGet("search_users")]
        public IActionResult GetUsersByName(string firstName, string lastName)
        {
            var response = _userService.GetUserByName(firstName, lastName);
               
            if (response == null)
                return BadRequest(new { message = "No user with the given name!" });

            return Ok(response);
        }

        [HttpDelete("clinic/{id}")]
        [Authorize(UserRole.SuperAdmin)]
        public IActionResult DeleteClinicById(int id)
        {
            try
            {
                var deletedClinic = _clinicService.DeleteClinicById(id);
                return Ok(deletedClinic);
            }
            catch (NullReferenceException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }

        }
    }
}
