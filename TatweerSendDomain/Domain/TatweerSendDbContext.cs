using Microsoft.EntityFrameworkCore;
using TatweerSendDomain.Domain;

namespace TatweerSendDomain
{


    public class TatweerSendDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ReasonRefuse> ReasonRefuses { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BankRegion> BankRegions { get; set; }
        public DbSet<BranchSetting> BranchSettings { get; set; }
        public DbSet<BranchWorkTime> BranchWorkTimes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderRequest> OrderRequests { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }

        //public TatweerSendDbContext()
        //{
        //}

        public TatweerSendDbContext(DbContextOptions<TatweerSendDbContext> contextOptions) : base(contextOptions)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder.UseSqlServer($"Data Source=.;Initial Catalog={nameof(TatweerSendDbContext)};Trusted_Connection=true");
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.Property(e => e.BranchNo).IsRequired();
                entity.Property(e => e.BranchRegionId).HasMaxLength(128);

                entity.HasOne(d => d.BranchRegion)
                    .WithMany(p => p.Branchs)
                    .HasForeignKey(d => d.BranchRegionId)
                    .HasConstraintName("FK_BRANCH_REGION");

            });

            modelBuilder.Entity<BranchSetting>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");

                entity.Property(e => e.AccountChekBook);
                entity.Property(e => e.CompanyFrom).IsRequired().HasDefaultValue(1);
                entity.Property(e => e.CompanyTo).IsRequired().HasDefaultValue(1000);
                entity.Property(e => e.CertifiedFrom).IsRequired().HasDefaultValue(1);
                entity.Property(e => e.CertifiedTo).IsRequired().HasDefaultValue(1000);
                entity.Property(e => e.IndividualFrom).IsRequired().HasDefaultValue(1);
                entity.Property(e => e.IndividualTo).IsRequired().HasDefaultValue(1000);
                entity.Property(e => e.BranchId).HasMaxLength(128);

                entity.Property(e => e.IndividualQuentityOfDay).IsRequired();
                entity.Property(e => e.IndividualRequestAccountDay).IsRequired();

                entity.HasOne(d => d.Branch)
                    .WithOne(p => p.BranchSetting)
                    .HasForeignKey<BranchSetting>(d => d.BranchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_BRANCH_Branch_Setting");

            });

            modelBuilder.Entity<BranchWorkTime>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.TimeStart).IsRequired();
                entity.Property(e => e.TimeEnd).IsRequired();

                entity.Property(e => e.DayName).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(d => d.Branch)
                   .WithMany(p => p.BranchWorkTimes)
                   .HasForeignKey(d => d.BranchId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Branch_Work_Times");
            });

            modelBuilder.Entity<Bank>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);

                entity.Property(e => e.BankNo).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);

            });



            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.RegionNo).IsRequired();
            });

            modelBuilder.Entity<BankRegion>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);

                entity.Property(e => e.BankId).HasMaxLength(128);
                entity.Property(e => e.RegionId).HasMaxLength(128);

                entity.HasOne(d => d.Bank)
                  .WithMany(p => p.BankRegions)
                  .HasForeignKey(d => d.BankId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_Region_Bank");

                entity.HasOne(d => d.Region)
                  .WithMany(p => p.BankRegions)
                  .HasForeignKey(d => d.RegionId)
                  .HasConstraintName("FK_Region_Bank_Region");
            });

            modelBuilder.Entity<Account>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.CreateAt).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ModifyAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);

                entity.Property(e => e.BranchId).HasMaxLength(128);
                entity.Property(e => e.AccountType).IsRequired();
                entity.Property(e => e.AccountName).IsRequired();
                entity.Property(e => e.AccountNo).IsRequired();
                entity.Property(e => e.AccountState).IsRequired();
                entity.Property(e => e.PhoneNumber);

                entity.HasOne(d => d.Branch)
                  .WithMany(p => p.Accounts)
                  .HasForeignKey(d => d.BranchId)
                  .HasConstraintName("FK_Accounts");
            });


            modelBuilder.Entity<OrderRequest>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.LastModifyDate).HasColumnType("datetime");
                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.EmployeeNo);

                entity.Property(e => e.BranchId).HasMaxLength(128);
                entity.Property(e => e.IdentityNumber);
                entity.Property(e => e.IdentityNumberBank);
                entity.Property(e => e.OrderRequestState).IsRequired();
                entity.Property(e => e.OrderRequestType).IsRequired();

                entity.HasOne(d => d.Branch)
                  .WithMany(p => p.OrderRequests)
                  .HasForeignKey(d => d.BranchId)
                  .HasConstraintName("FK_OrderRequest_Branchs");

            });


            modelBuilder.Entity<OrderItem>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.OrderRequestId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.AccountNo).IsRequired();
                entity.Property(e => e.AccountName).IsRequired();
                entity.Property(e => e.AccountId);
                entity.Property(e => e.SerialFrom);
                entity.Property(e => e.CountChekBook).IsRequired();
                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.HasOne(d => d.OrderRequest)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(d => d.OrderRequestId)
                  .HasConstraintName("FK_OrderItemt_Branchs");

            });
            modelBuilder.Entity<OrderEvent>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(128);
                entity.Property(e => e.OrderRequestId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.OrderCreationDate).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.EmployeeNo);
                entity.Property(e => e.UserType).IsRequired();

                entity.HasOne(d => d.OrderRequest)
                  .WithMany(p => p.OrderEvents)
                  .HasForeignKey(d => d.OrderRequestId)
                  .HasConstraintName("FK_OrderSend_Branchs");

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
