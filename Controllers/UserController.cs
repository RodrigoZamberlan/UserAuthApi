using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Contexts;
using UserAuthApi.Models;
using UserAuthApi.Utils; 

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
            return NotFound($"User with the id {id} not found");
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUserData) {
        if (!ModelValidator.Validate<User>(newUserData, out string errorMessage)) {
            return BadRequest(errorMessage);
        }

        var existingUser = await _context.Users.AnyAsync(u => u.Email == newUserData.Email);

        if (existingUser) {
            return Conflict($"Email {newUserData.Email} already exists");
        }
        
        _context.Users.Add(newUserData);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserById), new { id = newUserData.Id}, newUserData);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUserData) {
        if (!ModelValidator.Validate<User>(updatedUserData, out string errorMessage)) {
            return BadRequest(errorMessage);
        }
        
        if (updatedUserData == null || updatedUserData.Id == id) {
            return BadRequest("Invalid user data");
        }

        var userToUpdate = await _context.Users.FindAsync(updatedUserData.Id);
        if (userToUpdate == null) {
            return NotFound($"User {updatedUserData.Firstname} not found");
        }

        userToUpdate.Firstname = userToUpdate.Firstname;
        userToUpdate.Lastname = userToUpdate.Lastname;
        userToUpdate.Lastname = userToUpdate.Lastname;
        userToUpdate.Email = userToUpdate.Email;
        userToUpdate.Password = userToUpdate.Password;
        userToUpdate.Role = userToUpdate.Role;
        userToUpdate.Active = userToUpdate.Active;

        _context.Users.Update(userToUpdate);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id) {
        var userToDelete = await _context.Users.FindAsync(id);

        if (userToDelete == null) {
            return NotFound($"User with the id {id} not found");
        }

        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}