using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyDent.DataAccess;
using MyDent.DataAccess.Abstactions;
using MyDent.Domain.Models;
using MyDent.Domain.Request_Response;
using MyDent.Services.Abstractions;
using MyDent.Services.Exceptions;
using MyDent.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services
{
    public class UserService : IUserService
    {
        private readonly MyDentDbContext _dbContext;
        private readonly AppSettings _appSettings;
        private readonly IHashingString _hashingString;
        private readonly IEmailHandler _emailHandler;
        private readonly IQrCodeGenerator _qrCodeGenerator;
        public UserService(MyDentDbContext dbConetxt, IOptions<AppSettings> appSettings, IHashingString hashingString, IEmailHandler emailHandler, IQrCodeGenerator qrCodeGenerator)
        {
            _dbContext = dbConetxt;
            _appSettings = appSettings.Value;
            _hashingString = hashingString;
            _emailHandler = emailHandler;
            _qrCodeGenerator = qrCodeGenerator;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(22),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == model.Email && user.Password == model.Password);

            if (user == null) return null;

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public User AddNewUser(User userToAdd)
        {
            var userByEmail = _dbContext.Users.SingleOrDefault(x => x.Email == userToAdd.Email);

            if (userByEmail != null)
                return null;

            _dbContext.Users.Add(userToAdd);

            try
            {
                _dbContext.SaveChanges();
                _emailHandler.SendEmailToNewUser(userToAdd, _qrCodeGenerator);
            }
            catch (DatabaseException)
            {
                throw new DatabaseException("Could not save changes after add.");
            }
            return userToAdd;
        }

        public async Task UpdateUser(User updatedUser)
        {
            var userById = _dbContext.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (userById != null)
            {
                userById.FirstName = updatedUser.FirstName;
                userById.LastName = updatedUser.LastName;
                userById.Age = updatedUser.Age;
                userById.Email = updatedUser.Email;
                userById.PhoneNumber = updatedUser.PhoneNumber;
            }
            await _dbContext.SaveChangesAsync();
        }

        public void ChangePassword(int userId, string newPass)
        {
            /*if (newPass == null || newPass.Length == 0)
            {
                throw new NullPasswordException("You can't have an empty password!");
            }*/

            string hashedNewPass = _hashingString.HashString(newPass);
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);


            /*if (hashedNewPass == user.Password)
            {
                throw new SamePasswordException("You can't use your old password as your new password!");
            }*/

            user.Password = hashedNewPass;
            _dbContext.Users.Update(user);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Could not update user!");
            }
        }

        public User DeleteUserByEmail(string email)
        {
            var userByEmail = _dbContext.Users.SingleOrDefault(x => x.Email == email);

            if (userByEmail == null)
            {
                throw new UserException($"User with the given email was not found: {email}", 404);
            }

            _dbContext.Users.Remove(userByEmail);

            _dbContext.SaveChanges();

            return userByEmail;
        }

        public User GetUserByEmail(string email)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email.Equals(email));
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public List<User> GetUserByName(string firstName, string lastName)
        {
            List<User> users = new List<User>();
            if (firstName == "null" || lastName == "null")

            {
                users = _dbContext.Users.Where(u => u.FirstName==firstName || u.LastName==lastName).ToList();
            }
            else
            {
                users = _dbContext.Users.Where(u => u.FirstName==firstName && u.LastName==lastName).ToList();
            }
            
            if(users.Count == 0)
                return null;

            return users;
        }
    }
}