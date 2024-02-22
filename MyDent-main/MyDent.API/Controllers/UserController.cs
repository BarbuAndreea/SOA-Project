using Microsoft.AspNetCore.Mvc;
using MyDent.DataAccess.Abstactions;
using MyDent.Domain.DTO;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Domain.Request_Response;
using MyDent.Services.Abstractions;
using MyDent.Services.Helpers;
using System;

namespace MyDent.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IMedicService _medicService;
        private readonly IClinicAdminService _clinicAdminService;
        private readonly IHashingString _hashingString;

        public UserController(IUserService userService, IPatientService patientService, IMedicService medicService, IClinicAdminService clinicAdminService, IHashingString hashingString)
        {
            _userService = userService;
            _patientService = patientService;
            _medicService = medicService;
            _clinicAdminService = clinicAdminService;
            _hashingString = hashingString;
        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }

        [Authorize(UserRole.Medic, UserRole.Patient,UserRole.SuperAdmin)]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("newUser")]
        public IActionResult AddNewUser(UserDTO userDTO)
        {
            User user = (User)HttpContext.Items["User"];
            User userToAdd = new()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNumber = userDTO.PhoneNumber,
                Age = userDTO.Age,
                Email = userDTO.Email,
                Password = _hashingString.HashString(userDTO.Password),
                Role = userDTO.Role,
                PersonalCode = Guid.NewGuid().ToString().Substring(0, 8)
            };

            var response = _userService.AddNewUser(userToAdd);

            if (response == null)
                return BadRequest(new { message = "Email already exist" });

            if (userDTO.Role == UserRole.Patient)
            {
                _patientService.AddPatient(userToAdd);
            }
            else if (userDTO.Role == UserRole.Medic)
            {
                int clinicId = -1;
                if (user.Role == UserRole.ClinicAdmin)
                {
                    clinicId = _clinicAdminService.GetClinicAdminByUserId(user.Id).ClinicId;
                }
                else if (user.Role == UserRole.SuperAdmin)
                {
                    clinicId = userDTO.ClinicId;
                }

                Medic medicToAdd = new()
                {
                    UserM = userToAdd,
                    Specialization = userDTO.Specialization,
                    ClinicId = clinicId,
                    StartWorkingHour = new System.DateTime(),
                    EndWorkingHour = new System.DateTime()
                };

                _medicService.AddMedic(medicToAdd);
            }
            else if (userDTO.Role == UserRole.ClinicAdmin)
            {
                ClinicAdmin clinicA = new()
                {
                    UserC = userToAdd,
                    ClinicId = userDTO.ClinicId
                };

                _clinicAdminService.AddClinicAdmin(clinicA);
            }

            return Ok(response);
        }

        [HttpGet("{email}")]
        public ActionResult<User> GetUserByEmail(string email)
        {
            var response = _userService.GetUserByEmail(email);
            if (response == null)
                return BadRequest(new { message = "Email does not exist" });

            return Ok(response);
        }

        [HttpDelete("{email}")]
        public ActionResult<User> DeleteUserByEmail(string email)
        {
            var userByEmail = _userService.GetUserByEmail(email);

            _userService.DeleteUserByEmail(email);

            if (userByEmail.Role == UserRole.Patient)
            {
                _patientService.DeletePatientByUserId(userByEmail.Id);
            }
            else if (userByEmail.Role == UserRole.Medic)
            {
                _medicService.DeleteMedicByUserId(userByEmail.Id);
            }

            return Ok(userByEmail);
        }
    }
}
