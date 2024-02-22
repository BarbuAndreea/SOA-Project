using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDent.DataAccess;
using MyDent.Domain.DTO;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using MyDent.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyDent.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MedicController : ControllerBase
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
        private readonly IRadiographyService _radiographyService;
        private readonly IEmailHandler _emailHandler;
        private readonly MyDentDbContext _dbContext;

        public MedicController(IPatientService patientService, IInterventionService interventionService, IMedicService medicService, IAppointmentService appointmentService,
            IRoomService roomService, IHolidayService holidayService, IRadiographyService radiographyService, IUserService userService,
            IClinicService clinicService, IClinicAdminService clinicAdminService, IEmailHandler emailHandler, MyDentDbContext dbContext)
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
            _radiographyService = radiographyService;
            _emailHandler = emailHandler;
            _dbContext = dbContext;
        }

        [Authorize(UserRole.Medic, UserRole.Patient)]
        [HttpGet("get_medic")]
        public IActionResult GetMedicByUserId()
        {
            User user = (User)HttpContext.Items["User"];
            var medic = _medicService.GetMedicByUserId(user.Id);
            return Ok(medic);
        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin, UserRole.SuperAdmin)]
        [HttpGet("get_all_patients")]
        public IActionResult GetAllPatientsByClinic()
        {
            User user = (User)HttpContext.Items["User"];
            int clinicId = -1;
            if (user.Role == UserRole.Medic)
            {
                clinicId = _medicService.GetMedicByUserId(user.Id).ClinicId;
            }
            else if (user.Role == UserRole.ClinicAdmin)
            {
                clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            }
            var clinic = _clinicService.GetClinicById(clinicId);
            var patients = _patientService.GetAllPatientsByClinic(clinic);
            return Ok(patients);
        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpGet("search_patient")]
        public IActionResult GetPacientsByName(string firstName, string lastName)
        {
            User user = (User)HttpContext.Items["User"];
            int clinicId = -1;
            if (user.Role == UserRole.Medic)
            {
                clinicId = _medicService.GetMedicByUserId(user.Id).ClinicId;
            }
            else if (user.Role == UserRole.ClinicAdmin)
            {
                clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            }
            var clinic = _clinicService.GetClinicById(clinicId);
            try
            {
                var patients = _patientService.GetPatientsByName(firstName, lastName, clinic);
                return Ok(patients);
            }
            catch (UserException e)
            {
                return StatusCode(e.StatusCode, new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _patientService.GetPatientById(id);
            return Ok(patient);
        }

        [HttpGet("get_patient_by_user/{id}")]
        public IActionResult GetPatientByUserId(int id)
        {
            var patient = _patientService.GetPatientByUserId(id);
            return Ok(patient);
        }

        [HttpGet("pacientIntervention/{id}")]
        public ActionResult<Intervention> GetInterventionsByPatientId(int id)
        {
            var interventions = _interventionService.GetInterventionByPatient(id);
            return Ok(interventions);
        }


        [Authorize(UserRole.Medic)]
        [HttpPost("addIntervention")]
        public ActionResult<Intervention> AddNewIntervention([FromBody] Intervention intervention)
        {
            User user = (User)HttpContext.Items["User"];
            intervention.MedicId = _medicService.GetMedicByUserId(user.Id).Id;

            var response = _interventionService.AddNewIntervention(intervention);

            return Ok(response);

        }

        [Authorize(UserRole.Medic)]
        [HttpPost("updateIntervention")]
        public ActionResult<Intervention> UpdateIntervention([FromBody] Intervention intervention)
        {
            intervention.Date = ConvertToEESTFroGMT(intervention.Date);

            User user = (User)HttpContext.Items["User"];
            intervention.MedicId = _medicService.GetMedicByUserId(user.Id).Id;

            var response = _interventionService.UpdateIntervention(intervention);

            return Ok(response);

        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpPost("addAppointment")]
        public ActionResult<Appointment> AddNewAppointment([FromBody] Appointment appointment)
        {
            appointment.StartDate = ConvertToEESTFroGMT(appointment.StartDate);
            appointment.EndDate = ConvertToEESTFroGMT(appointment.EndDate);

            User user = (User)HttpContext.Items["User"];
            if (user.Role == UserRole.Medic)
            {
                appointment.MedicId = _medicService.GetMedicByUserId(user.Id).Id;
            }

            try
            {
                var response = _appointmentService.AddAppointment(appointment);
                //_emailHandler.SendEmailAppoitment(response, user, ActionTypeEnum.New);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpPost("updateAppointment")]
        public ActionResult<Appointment> UpdateAppointment([FromBody] Appointment appointment)
        {
            appointment.StartDate = ConvertToEESTFroGMT(appointment.StartDate);
            appointment.EndDate = ConvertToEESTFroGMT(appointment.EndDate);

            User user = (User)HttpContext.Items["User"];
            Appointment response = _appointmentService.GetAppointmentById(appointment.Id);
            try
            {
                if (response.StartDate != appointment.StartDate || response.EndDate != appointment.EndDate)
                {
                    _emailHandler.SendEmailAppoitment(appointment, user, ActionTypeEnum.Edit);
                }
                _appointmentService.UpdateAppointment(appointment);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok(response);
        }

        [Authorize(UserRole.Medic)]
        [HttpPost("updateAppointmentStatus")]
        public ActionResult<Appointment> UpdateAppointmentStatus([FromBody] Appointment appointment)
        {
            User user = (User)HttpContext.Items["User"];
            appointment.MedicId = _medicService.GetMedicByUserId(user.Id).Id;

            var response = _appointmentService.UpdateAppointmentStatus(appointment);

            return Ok(response);

        }

        private DateTime ConvertToEESTFroGMT(DateTime date)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, easternZone);
        }

        [HttpGet("get-today-appointments")]
        public ActionResult<Appointment> GetTodayAppointmets()
        {
            var appointments = _appointmentService.GetTodayAppointments();
            return Ok(appointments);
        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpPost("get-appointments-by-date")]
        public ActionResult<Appointment> GetAppointmetsByDateAndMedicId([FromBody] DateTime date)
        {
            var user = (User)HttpContext.Items["User"];
            var id = user.Role == UserRole.Medic ? _medicService.GetMedicByUserId(user.Id).Id : (user.Role==UserRole.ClinicAdmin ? _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId : -1);
            if (id==-1)
                return BadRequest();
            try
            {
                var appointments = new List<Appointment>();
                if (user.Role==UserRole.Medic)
                {
                    appointments = _appointmentService.GetAppointmentsByDateAndMedicId(date, id);
                }
                else if (user.Role==UserRole.ClinicAdmin)
                {
                    appointments = _appointmentService.GetAppointmentsByDateAndClinicId(date, id);
                }
                return Ok(appointments);
            }
            catch (ValidationException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [Authorize(UserRole.Patient, UserRole.Medic)]
        [HttpGet("get_all_appointments")]
        public IActionResult GetAllAppointments()
        {
            var users = _appointmentService.GetAllAppointments();
            return Ok(users);
        }

        [Authorize(UserRole.Medic)]
        [HttpGet("appointments_by_status/{status}")]
        public ActionResult<List<Appointment>> GetAppointmentByStatus(AppointmentStatus status)
        {
            int userId = ((User)HttpContext.Items["User"]).Id;
            var medicId = _medicService.GetMedicByUserId(userId).Id;

            try
            {
                var x = _appointmentService.GetMedicAppointmentsByStatus(status, medicId);
                return Ok(x);
            }
            catch (Exception e)
            {
                return NotFound(new { message = e.Message });
            }

        }

        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpDelete("appointment/{id}")]
        public IActionResult DeleteAppointmentById(int id)
        {
            try
            {
                var deletedAppointment = _appointmentService.DeleteAppointmentById(id);
            }
            catch (NullReferenceException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }
            return Ok();
        }


        [Authorize(UserRole.Medic, UserRole.ClinicAdmin)]
        [HttpGet("get-rooms-by-clinic")]
        public ActionResult<List<Room>> GetRoomsByClinic()
        {
            User user = ((User)HttpContext.Items["User"]);
            int clinicId = -1;
            if (user.Role == UserRole.Medic)
            {
                clinicId = _medicService.GetMedicByUserId(user.Id).ClinicId;
            }
            else if (user.Role == UserRole.ClinicAdmin)
            {
                clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
            }

            var rooms = _roomService.GetRoomsByClinic(clinicId);
            return Ok(rooms);
        }

        [Authorize(UserRole.Medic, UserRole.Patient)]
        [HttpGet("holidays/{medicId}")]
        public IActionResult GetHolidaysByMedicId(int medicId)
        {
            return Ok(_holidayService.GetAllHolidaysByMedicId(medicId));
        }

        [Authorize(UserRole.Medic)]
        [HttpPost("add-holiday")]
        public IActionResult AddHoliday([FromBody] Holiday holiday)
        {
            UserRole userRole = ((User)HttpContext.Items["User"]).Role;
            if (userRole == UserRole.Medic)
            {
                User user = ((User)HttpContext.Items["User"]);
                Medic medic = _medicService.GetMedicByUserId(user.Id);
                holiday.MedicId = medic.Id;
            }
            try
            {
                var holidayToAdd = _holidayService.AddHoliday(holiday);
                return Ok(holidayToAdd);
            }
            catch (ValidationException error)
            {
                return BadRequest(new { message = error.Message });
            }
        }

        [Authorize(UserRole.Medic)]
        [HttpPost("update-holiday")]
        public IActionResult UpdateHoliday([FromBody] Holiday holiday)
        {
            UserRole userRole = ((User)HttpContext.Items["User"]).Role;
            if (userRole == UserRole.Medic)
            {
                User user = ((User)HttpContext.Items["User"]);
                Medic medic = _medicService.GetMedicByUserId(user.Id);
                holiday.MedicId = medic.Id;
            }
            try
            {
                var holidayToAdd = _holidayService.UpdateHoliday(holiday);
                return Ok(holidayToAdd);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [Authorize(UserRole.Medic, UserRole.Patient, UserRole.ClinicAdmin)]
        [HttpPut("change_password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDTO changedPassword)
        {
            int userId = ((User)HttpContext.Items["User"]).Id;
            _userService.ChangePassword(userId, changedPassword.Password);
            return StatusCode((int)HttpStatusCode.OK);

        }

        [Authorize(UserRole.Medic)]
        [HttpGet("get_clinic_by_medic/{medic}")]
        public IActionResult GetClinicByMedic(Medic medic)
        {
            return Ok(_clinicService.GetClinicByMedic(medic));
        }

        [Authorize(UserRole.Medic)]
        [HttpPost("update_medic_info")]
        public async Task<IActionResult> UpdateMedicInfo([FromBody] Medic medic)
        {
            medic.StartWorkingHour = ConvertToEESTFroGMT(medic.StartWorkingHour);
            medic.EndWorkingHour = ConvertToEESTFroGMT(medic.EndWorkingHour);
            try
            {
                await _userService.UpdateUser(medic.UserM);
                await _medicService.UpdateMedic(medic);
            }
            catch (Exception error)
            {
                var x = error;
            }

            return Ok();
        }

        [HttpPost("upload/{id}"), DisableRequestSizeLimit]
        public IActionResult Upload(int id)
        {
            try
            {
                var patient = _patientService.GetPatientById(id);
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (patient.Radiographies == null)
                    {
                        patient.Radiographies = new List<Radiography>();
                    }
                    var imageArray = System.IO.File.ReadAllBytes(fullPath);
                    patient.Radiographies.Add(new Radiography { Image64 = Convert.ToBase64String(imageArray), Date = DateTime.Now });
                    _dbContext.SaveChanges();
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("holiday/{id}")]
        [Authorize(UserRole.Medic)]
        public IActionResult DeleteHolidayById(int id)
        {
            try
            {
                var deletedHoliday = _holidayService.DeleteHolidayById(id);
                return Ok(deletedHoliday);
            }
            catch (NullReferenceException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }

        }
    }
}
