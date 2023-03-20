using Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Domain
{


    public class IdentityTatweerSendDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permisstion> Permisstions { get; set; }
        public DbSet<RolePermisstion> RolePermisstions { get; set; }

        //public IdentityTatweerSendDB()
        //{

        //}

        public IdentityTatweerSendDB(DbContextOptions<IdentityTatweerSendDB> contextOptions) : base(contextOptions)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder.UseSqlServer($"Data Source=.;Initial Catalog={nameof(IdentityTatweerSendDB)};Trusted_Connection=true");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.ModifyAt).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();
            });


            modelBuilder.Entity<Permisstion>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RolePermisstion>(entity =>
            {


                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.PermisstionId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Permisstion)
                    .WithMany(p => p.RolePermisstions)
                    .HasForeignKey(d => d.PermisstionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_RolePermisstion_Permisstions");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermisstions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_RolePermisstion_Roles");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.CreateAt).HasColumnType("datetime").HasDefaultValue(DateTime.Now);
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.SendSMS).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.EmployeeNumber);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.RoleId).HasMaxLength(128);
                entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
