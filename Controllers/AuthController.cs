using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Contexts;
using UserAuthApi.Helpers;
using UserAuthApi.DTOs;

namespace UserAuthApi.Controllers; 

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase {
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration) {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto) {
        var existingUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == userLoginDto.Email && user.Password == userLoginDto.Password);

        if (existingUser == null) {
            return Unauthorized("Invalid email or password");
        }

        var token = JwtTokenGenerator.GenerateJwtToken(existingUser, _configuration);
        return Ok(new { token });
    }
}