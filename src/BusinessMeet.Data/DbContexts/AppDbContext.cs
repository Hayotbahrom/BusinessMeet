using BusinessMeet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Meet> Meets { get; set; }
    public DbSet<Company> Companys { get; set; }
    public DbSet<UserMeet> UsersMeets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
        .HasMany(c => c.Meets)
        .WithOne(s => s.Company)
        .HasForeignKey(s => s.CompanyId);
    }
}
