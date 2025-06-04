using BloodDonationAppUserService.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAppUserService.Context
{
    //sa
    //YourStrong!Passw0rd
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RequestModel>Requests{ get; set; }
        
        public DbSet<DonationModel> Donations{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Mail).IsUnique();
                entity.HasIndex(u => u.Tc).IsUnique();
                entity.HasAlternateKey(u => u.Tc);

                entity.Property(u => u.BloodType).HasConversion<string>();
                entity.Property(u => u.City).HasConversion<string>();
            });

            modelBuilder.Entity<RequestModel>(entity =>
            {
                entity.ToTable("Requests");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.BloodType).HasConversion<string>();
                entity.Property(r => r.City).HasConversion<string>();
                entity.Property(r => r.UrgencyLevel).HasConversion<string>();
                entity.Property(r => r.IsActive);

                entity
                   .HasOne<UserModel>()
                   .WithMany()
                   .HasForeignKey(r => r.UserTc)
                   .HasPrincipalKey(u => u.Tc);
            });
        }
    }
}