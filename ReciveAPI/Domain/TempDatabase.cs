using Microsoft.EntityFrameworkCore;

namespace ReciveAPI.Domain
{
    public partial class TempDatabase : DbContext
    {
        private string userName = "sa";
        private string Password = "123456";
        private string dataBasename = "TempDatabase";
        private string serverName = ".";
        //public TempDatabase()
        //{
        //}


        public TempDatabase(DbContextOptions<TempDatabase> options)
            : base(options)
        {
        }

        public virtual DbSet<Table> Tables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server={serverName};Database={dataBasename} ;user id={userName};password={Password};Integrated Security=false;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>(entity =>
            {
                entity.ToTable("Table");

                entity.Property(e => e.AccountName).HasColumnName("Account_Name");

                entity.Property(e => e.AccountNumber).HasColumnName("Account_Number");

                entity.Property(e => e.AothrizedOrdersDate)
                    .HasColumnType("datetime")
                    .HasColumnName("AothrizedOrders_Date");

                entity.Property(e => e.BranchName).HasColumnName("Branch_Name");

                entity.Property(e => e.BranchNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Branch_Number");

                entity.Property(e => e.ChCount).HasColumnName("Ch_Count");

                entity.Property(e => e.FromSerial).HasColumnName("From_Serial");

                entity.Property(e => e.RegName)
                    .HasMaxLength(50)
                    .HasColumnName("Reg_Name");

                entity.Property(e => e.RegionNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Region_Number");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Request_Date");

                entity.Property(e => e.RequestIdentity).HasColumnName("Request_Identity");

                entity.Property(e => e.RequestStatus).HasColumnName("Request_Status");

                entity.Property(e => e.Tc).HasColumnName("TC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
