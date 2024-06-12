using ElderHomeMonitoringSystem.DTOs;
using ElderHomeMonitoringSystem.Interfaces;
using ElderHomeMonitoringSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using ElderHomeMonitoringSystem.Exceptions;

namespace ElderHomeMonitoringSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> Login(LoginDTO loginDto)
        {
            var user = await _accountRepository.GetUserByUsername(loginDto.Username);
            if (user == null)
            {
                throw new AccountExceptions.UserNotFoundException("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new AccountExceptions.PasswordMismatchException("Invalid password");
                }
            }

            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO newUserDto)
        {
            if (newUserDto == null)
            {
                return BadRequest(ModelState);
            }

            if (await _accountRepository.UserExits(newUserDto.Username))
            {
                throw new AccountExceptions.UsernameTakenException("Username is taken!");
            }

            if (await _accountRepository.EmailExists(newUserDto.Email))
            {
                throw new AccountExceptions.EmailTakenException("Email already in use for another account!");
            }

            byte[] passwordSalt;
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
            }

            byte[] passwordHash;
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newUserDto.Password));
            }

            var user = new User
            {
                Username = newUserDto.Username,
                Address = newUserDto.Address,
                FirstName = newUserDto.FirstName,
                LastName = newUserDto.LastName,
                Phone = newUserDto.Phone,
                Email = newUserDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ProfileImage = newUserDto.ProfileImage
            };

            if (!await _accountRepository.AddUser(user))
            {
                throw new Exception("Process interrupted! Couldn't add user");
            }

            return Ok(user);
        }

        [HttpPost("forgetPassword")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDto)
        {
            if (forgetPasswordDto == null)
            {
                return BadRequest(ModelState);
            }

            var user = await _accountRepository.GetUserByUsername(forgetPasswordDto.Username);
            if (user == null)
            {
                throw new AccountExceptions.UserNotFoundException("User not found");
            }

            byte[] newPasswordHash;
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                newPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(forgetPasswordDto.NewPassword));
            }

            user.PasswordHash = newPasswordHash;

            if (!await _accountRepository.UpdateUser(user))
            {
                throw new Exception("Process interrupted! Couldn't update user password");
            }

            return Ok(user);
        }

        [HttpGet("User/{username}")]
        public async Task<ActionResult<int>> GetUserID(string username)
        {
            var id = await _accountRepository.GetUserIDByUsername(username);
            if (id == 0)
            {
                throw new Exception("Something went wrong while retrieving the id!");
            }
            return id;
        }

        [HttpGet("User/username/{id}")]
        public async Task<ActionResult<string>> GetUsernameID(int id)
        {
            var username = await _accountRepository.GetUsernamaByID(id);
            return username;
        }

        [HttpDelete("Delete/{userid}")]
        public async Task<ActionResult<bool>> RemoveUser(int userid)
        {
            var user = await _accountRepository.GetUserByID(userid);
            if (user != null)
                return await _accountRepository.RemoveUser(user);
            else
                return BadRequest();
        }

        [HttpPut("Update")]
        public async Task<ActionResult<bool>> UpdateUser(User user)
        {
            if (await _accountRepository.UserExits(user))
            {
                throw new AccountExceptions.UsernameTakenException("Username is taken!");
            }

            if (await _accountRepository.EmailExists(user))
            {
                throw new AccountExceptions.EmailTakenException("Email already in use for another account!");
            }

            return await _accountRepository.UpdateUser(user);
        }

    }
}