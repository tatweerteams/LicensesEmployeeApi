using Microsoft.EntityFrameworkCore;

namespace LogginDomain
{
    public class LogginContextDB : DbContext
    {


        public DbSet<LogginData> LogginData { get; set; }

        //public LogginContextDB()
        //{
        //}

        public LogginContextDB(DbContextOptions<LogginContextDB> contextOptions) : base(contextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder.UseSqlServer($"Data Source=.;Initial Catalog={nameof(LogginContextDB)};Trusted_Connection=true");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogginData>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);
            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
