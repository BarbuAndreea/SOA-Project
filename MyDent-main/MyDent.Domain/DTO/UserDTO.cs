using MyDent.Domain.Enum;

namespace MyDent.Domain.DTO
{
    public class UserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }

        public SpecializationEnum Specialization { get; set; }

        public int ClinicId { get; set; }
    }
}
