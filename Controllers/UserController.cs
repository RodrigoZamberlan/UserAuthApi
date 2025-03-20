using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Contexts;
using UserAuthApi.DTOs;
using UserAuthApi.Helpers;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers;

/*
    IMPROVEMENTS TO DO
    1. Encrypt the password before save in the database
*/

[Route("api/users")]
[ApiController]
public class UserController: ControllerBase {
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserController(AppDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers() {
        var isAuthorized = UserClaims.IsAdmin(User);
        if (!isAuthorized)
        {
            return Unauthorized("Only user's with the admin role can access the list of all user's");
        }

        var users = await _context.Users
        .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
        .ToListAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id) {
        var user = await _context.Users.FindAsync(id);

        if (user == null) {
            return NotFound($"User with the id {id} not found");
        }

        var isAuthorized = UserClaims.IsAuthorizedOrAdmin(User, id);
        if (!isAuthorized)
        {
            return Unauthorized("You can only access your own user info unless you're an admin user");
        }

        var userDto = _mapper.Map<UserDTO>(user);

        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUserData) {
        bool existingUser = await _context.Users.AnyAsync(u => u.Email == newUserData.Email);

        if (existingUser) {
            return Conflict($"Email {newUserData.Email} already exists");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUserData.Password);
        newUserData.Password = hashedPassword;
        
        _context.Users.Add(newUserData);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUserById), new { id = newUserData.Id}, newUserData);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUserData) {
        var isAuthorized = UserClaims.IsAuthorizedOrAdmin(User, id);
        if (!isAuthorized) {
            return Unauthorized("You can only edit your own user profile unless you're an admin user");
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

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id) {
        var isAuthorized = UserClaims.IsAuthorizedOrAdmin(User, id);
        if (!isAuthorized)
        {
            return Unauthorized("You can only access your own user info unless you're an admin");
        }

        var userToDelete = await _context.Users.FindAsync(id);

        if (userToDelete == null) {
            return NotFound($"User with the id {id} not found");
        }

        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}