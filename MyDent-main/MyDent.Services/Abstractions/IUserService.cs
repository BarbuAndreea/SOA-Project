using MyDent.Domain.DTO;
using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Domain.Request_Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDent.Services.Abstractions
{
    public interface IUserService
    {
        string GenerateJwtToken(User user);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        List<User> GetAllUsers();
        User GetUserById(int id);
        User AddNewUser(User userToAdd);
        User DeleteUserByEmail(string email);
        User GetUserByEmail(string email);

        void ChangePassword(int userId, string newPass);
        Task UpdateUser(User newUser);
        List<User> GetUserByName(string firstName, string lastName);
    }
}
