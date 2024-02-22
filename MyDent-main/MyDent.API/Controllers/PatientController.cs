using Microsoft.AspNetCore.Mvc;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using MyDent.Services.Helpers;

namespace MyDent.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMedicService _medicService;
        private readonly IAppointmentService _appointmentService;
        private readonly IInterventionService _interventionService;
        private readonly IRoomService _roomService;
        private readonly IClinicService _clinicService;

        public PatientController(IPatientService patientService, IMedicService medicService, IRoomService roomService, IClinicService clinicService, IAppointmentService appointmentService, IInterventionService interventionService)
        {
            _patientService = patientService;
            _medicService = medicService;
            _appointmentService = appointmentService;
            _interventionService = interventionService;
            _roomService = roomService;
            _clinicService = clinicService;
        }

        [Authorize(UserRole.Medic, UserRole.Patient)]
        [HttpGet("get_patient")]
        public IActionResult GetPatientByUserId()
        {
            User user = (User)HttpContext.Items["User"];
            var patient = _patientService.GetPatientByUserId(user.Id);
            return Ok(patient);
        }

        [Authorize(UserRole.Medic, UserRole.Patient, UserRole.ClinicAdmin)]
        [HttpGet("get_room/{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _roomService.GetRoomById(id);
            return Ok(room);
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<Patient> DeleteUserByEmail(int id)
        {
            try
            {
                var deletedPatient = _patientService.DeletePatientByUserId(id);
                return Ok(deletedPatient);
            }
            catch (UserException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        [Authorize(UserRole.Patient)]
        [HttpGet("get_next_appointments_by_patient")]
        public ActionResult<Appointment[]> GetNextAppointmentsByPatientId()
        {
            User user = (User)HttpContext.Items["User"];
            Patient patient = _patientService.GetPatientByUserId(user.Id);
            var appointments = _appointmentService.GetNextAppointmentsByPatientId(patient.Id);
            if (appointments.Count == 0)
                return NotFound(new { message = "You have have no next appoitments!" });
            return Ok(appointments);
        }

        [Authorize(UserRole.Patient)]
        [HttpGet("get_past_appointments_by_patient")]
        public ActionResult<Appointment[]> GetPastAppointmentsByPatientId()
        {
            User user = (User)HttpContext.Items["User"];
            Patient patient = _patientService.GetPatientByUserId(user.Id);
            var appointments = _appointmentService.GetPastAppointmentsByPatientId(patient.Id);
            if (appointments.Count == 0)
                return NotFound(new { message = "You have have no past appoitments!" });
            return Ok(appointments);
        }


        [HttpGet("patinet_intervetions")]
        public ActionResult<Intervention[]> GetInterventionsByPatient()
        {
            User user = (User)HttpContext.Items["User"];
            Patient patient = _patientService.GetPatientByUserId(user.Id);
            var interventions = _interventionService.GetInterventionByPatient(patient.Id);
            if (interventions.Count == 0)
                return NotFound(new { message = "There is no intervention yet!" });
            return Ok(interventions);
        }

        [Authorize(UserRole.Patient)]
        [HttpGet("get_all_medics")]
        public IActionResult GetAllMedics()
        {
            var medics = _medicService.GetAllMedics();
            return Ok(medics);
        }

        [Authorize(UserRole.Patient, UserRole.ClinicAdmin, UserRole.Medic)]
        [HttpGet("get_medic_by_id/{medicId}")]
        public IActionResult GetMedicById(int medicId)
        {
            var medic = _medicService.GetMedicById(medicId);
            return Ok(medic);
        }

        [Authorize(UserRole.Patient)]
        [HttpGet("get_clinics_by_patient")]
        public IActionResult GetAllPatientsByClinic()
        {
            User user = (User)HttpContext.Items["User"];
            var patient = _patientService.GetPatientByUserId(user.Id);
            var clinics = _clinicService.GetAllClinicsByPatient(patient);

            if (clinics == null)
                return NotFound(new {message = "You are not yet enrolled in a clinic" });

            return Ok(clinics);
        }

    }
}
