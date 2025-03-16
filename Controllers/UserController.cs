using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Contexts;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers;

[Route("api/users")]
[ApiController]
public class UserController: ControllerBase {
    private readonly AppDbContext _context;

    public UserController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers() {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id) {
        var user = await _context.Users.FindAsync(id);

        if (user == null) {
            return NotFound($"User with {id} not found");
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        var existingUser = await _context.Users.AnyAsync(u => u.Email == user.Email);

        if (existingUser) {
            return Conflict($"Email {user.Email} already exists.");
        }
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id}, user);
    }
}