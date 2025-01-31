using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Data;
using ToDoList.DTO;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountService(ApplicationDbContext appDbContext, TokenService tokenService, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _config = config;
        }

        public string Register(UserRegisterDto request)               
        {
            var existingUser = _appDbContext.Users.Any(u => u.Email == request.Email);
            if (existingUser)
            {
                return null;
            }
            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };


            _appDbContext.Add(user);
            _appDbContext.SaveChanges();
            var token = _tokenService.GenerateToken(user);

            return token;

        }

        public string Login(UserLoginDto request)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }
            var token = _tokenService.GenerateToken(user);
            return token;
        }

    }
}
