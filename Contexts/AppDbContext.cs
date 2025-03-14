using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;

namespace UserAuthApi.Contexts;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}

    public DbSet<User> Users { get; set; }
}