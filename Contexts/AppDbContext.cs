using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;

namespace UserAuthApi.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    
    DbSet<User> Users { get; set; }
}