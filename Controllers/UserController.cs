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

    

}