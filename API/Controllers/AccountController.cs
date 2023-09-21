using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;

            _context = context;
        }
        [HttpPost("register")]// api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDtos registerDto)
        {
            if (await UserExists(registerDto.username)) return BadRequest("user name is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
                PasswordSalt = hmac.Key

            };
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                Username= user.UserName,
                Token=_tokenService.CreateToken(user)

            };

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.UserName == loginDto.username);
            if (user == null) return Unauthorized("invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized(" invalid Password");
            }
                return new UserDto{
                Username= user.UserName,
                Token=_tokenService.CreateToken(user)

            };


        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.users.AnyAsync(x => x.UserName == username);
        }
    }
}
