using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using UserAuth.Business.Services;
using UserAuth.Data.Models;

namespace UserAuth.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AuthService _authService;

        public UserController(IUserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] NewUser Nuser)
        {
            User user = new User
            {
                FirstName = Nuser.FirstName,
                LastName = Nuser.LastName,
                Email = Nuser.Email,
                PasswordHash = Nuser.Password
            };

            _userService.RegisterUser(user);
            return Ok(new { Success = true, Message = "User registered successfully", Data = new { } }); 
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            bool isValid = _userService.VerifyUser(request.Email, request.Password);
            if (!isValid)
                return Unauthorized(new { Success = false, Message = "Invalid credentials", Data= new { } });

            var token = _authService.GenerateToken(request.Email);
            return Ok(new { Success = true, Message = "Login successful", Token = token });
        }
        [HttpGet("profile")]
        [Authorize] 
        public IActionResult GetProfile()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(new { Success = true, Message = "User profile data", Email = userEmail });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class NewUser {
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
        
        
}
