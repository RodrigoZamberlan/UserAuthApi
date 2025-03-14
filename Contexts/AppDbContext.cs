using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    //Define my tables here
}