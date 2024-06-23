using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace postgresTest.Model
{
  public class ApplicationDbContext : IdentityDbContext<User>
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Study> Studies { get; set; }
    public DbSet<Note> Notes { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //   base.OnModelCreating(builder);

    //   builder.Entity<Study>()
    //   .HasOne(s => s.User)
    //   .WithMany(u => u.Studies)
    //   .HasForeignKey(s => s.UserId);
    // }
  }

}