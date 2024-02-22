using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Helpers;
using MyDent.Services.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MyDent.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClinicAdminController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IInterventionService _interventionService;
        private readonly IMedicService _medicService;
        private readonly IAppointmentService _appointmentService;
        private readonly IRoomService _roomService;
        private readonly IHolidayService _holidayService;
        private readonly IUserService _userService;
        private readonly IClinicService _clinicService;
        private readonly IClinicAdminService _clinicAdminService;

        public ClinicAdminController(IPatientService patientService, IInterventionService interventionService, IMedicService medicService, IAppointmentService appointmentService,
            IRoomService roomService, IHolidayService holidayService, IUserService userService, IClinicService clinicService, IClinicAdminService clinicAdminService)
        {
            _interventionService= interventionService;
            _patientService = patientService;
            _medicService = medicService;
            _appointmentService = appointmentService;
            _roomService = roomService;
            _holidayService = holidayService;
            _userService = userService;
            _clinicService = clinicService;
            _clinicAdminService = clinicAdminService;
        }

        /*[Authorize(UserRole.Medic, UserRole.Patient)]
        [HttpGet("get_medic")]
        public IActionResult GetClinicAdminByUserId()
        {
            User user = (User)HttpContext.Items["User"];
            var clinic = _medicService.GetMedicByUserId(user.Id);
            return Ok(medic);
        }*/

        [Authorize(UserRole.ClinicAdmin)]
        [HttpPost("add_room")]
        public IActionResult AddRoom([FromBody] Room roomToAdd)
        {
            User user = (User)HttpContext.Items["User"];
            roomToAdd.ClinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;

            var response = _roomService.AddRoom(roomToAdd);

            return Ok(response);
        }


        [Authorize(UserRole.ClinicAdmin)]
        [HttpGet("get_rooms_by_clinic")]
        public IActionResult GetRoomsByClinic()
        {
            User user = (User)HttpContext.Items["User"];
            var clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;

            var response = _roomService.GetRoomsByClinic(clinicId);

            return Ok(response);
        }

        [HttpDelete("room/{id}")]
        [Authorize(UserRole.ClinicAdmin)]
        public IActionResult DeleteRoomById(int id)
        {
            try
            {
                var deletedRoom = _roomService.DeleteRoomById(id);
                return Ok(deletedRoom);
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpPut("edit_room")]
        [Authorize(UserRole.ClinicAdmin)]
        public IActionResult UpdateRoom([FromBody] Room room)
        {
            User user = (User)HttpContext.Items["User"];
            var clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            room.ClinicId = clinicId;
            try
            {
                var updatedRoom = _roomService.UpdateRoom(room);
                return Ok(updatedRoom);
            }
            catch (NullReferenceException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [Authorize(UserRole.ClinicAdmin, UserRole.Medic)]
        [HttpGet("add_patient_to_clinic/{patientId}")]
        public async Task<ActionResult<Patient>> ImportPatient(string patientId)
        {
            User user = (User)HttpContext.Items["User"];
            var clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            var clinic = _clinicService.GetClinicById(clinicId);

            try
            {
                return await _patientService.AddClinicToPatient(patientId, clinic);
            }
            catch (Exception e)
            {
                return BadRequest( new { message = e.Message });
            }
            
        }

        [Authorize(UserRole.ClinicAdmin)]
        [HttpGet("get_medics_by_clinic")]
        public IActionResult GetMedicsByClinic()
        {
            User user = (User)HttpContext.Items["User"];
            var clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            return Ok(_medicService.GetMedicsByClinic(clinicId));
        }
    }
}
