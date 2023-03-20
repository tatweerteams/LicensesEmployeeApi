using Microsoft.EntityFrameworkCore;

namespace TrackingServices.Domain
{
    public partial class ReciveDbContext : DbContext
    {

        private string userName = "sa";
        private string Password = "123456";
        private string dataBasename = "AljmhoriaReciveDB";
        private string serverName = ".";
        //public ReciveDbContext()
        //{
        //}

        public ReciveDbContext(DbContextOptions<ReciveDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FilesTbl> FilesTbls { get; set; } = null!;
        public virtual DbSet<UserPrinting> UserPrintings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server={serverName};Database={dataBasename} ;user id={userName};password={Password};Integrated Security=false;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilesTbl>(entity =>
            {
                entity.ToTable("FilesTBL");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<UserPrinting>(entity =>
            {
                entity.ToTable("UserPrinting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.TimeDownloaded).HasColumnType("datetime");

                entity.HasOne(d => d.Files)
                    .WithMany(p => p.UserPrintings)
                    .HasForeignKey(d => d.FilesId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserPrinting_FilesTBL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
