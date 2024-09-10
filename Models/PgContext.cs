using Microsoft.EntityFrameworkCore;

public class PgDbContext : DbContext
{
    public PgDbContext(DbContextOptions<PgDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

}
