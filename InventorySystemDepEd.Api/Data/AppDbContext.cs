using InventorySystemDepEd.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemDepEd.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
    : DbContext(options)
    {
        private readonly IConfiguration _config = config;   
        public DbSet<UsersModel> Users { get; set; } = null!;
        public DbSet<PersonnelsModel> Personnels { get; set; } = null!;
        public DbSet<PersonnelTransferModel> PersonnelTransfers { get; set; }
        public DbSet<OfficesModel> Offices { get; set; } = null!;
        public DbSet<DepartmentsModel> Departments { get; set; } = null!;
        public DbSet<PositionsModel> Positions { get; set; } = null!;
        public DbSet<RolesModel> Roles { get; set; } = null!;
        public DbSet<NotificationsModel> Notifications { get; set; }
        public DbSet<SuppliersModel> Suppliers { get; set; }

        //public AppDbContext(DbContextOptions<AppDbContext> options)
        //    : base(options)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Departments
            modelBuilder.Entity<DepartmentsModel>()
                .ToTable("tbl_departments")
                .HasKey(d => d.DepartmentId);

            modelBuilder.Entity<DepartmentsModel>()
                .HasMany(d => d.Offices)
                .WithOne(o => o.Department)
                .HasForeignKey(o => o.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Offices
            modelBuilder.Entity<OfficesModel>()
                .ToTable("tbl_offices")
                .HasKey(o => o.OfficeId);

            modelBuilder.Entity<OfficesModel>()
                .HasMany(o => o.Personnels)
                .WithOne(p => p.Office)
                .HasForeignKey(p => p.OfficeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Positions
            modelBuilder.Entity<PositionsModel>()
                .ToTable("tbl_positions")
                .HasKey(p => p.PositionId);

            modelBuilder.Entity<PositionsModel>()
                .HasMany(p => p.Personnels)
                .WithOne(pe => pe.Position)
                .HasForeignKey(pe => pe.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Personnels (USER RELATION ONLY HERE)
            modelBuilder.Entity<PersonnelsModel>()
                .ToTable("tbl_personnels")
                .HasKey(p => p.PersonnelId);

            modelBuilder.Entity<PersonnelsModel>()
                .HasOne(p => p.User)
                .WithOne(u => u.Personnel)
                .HasForeignKey<PersonnelsModel>(p => p.AccountID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonnelsModel>()
                .HasOne(p => p.Position)
                .WithMany(pos => pos.Personnels)
                .HasForeignKey(p => p.PositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users (ONLY ROLE HERE)
            modelBuilder.Entity<UsersModel>()
                .ToTable("tbl_users")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UsersModel>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.UserRole)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Roles
            modelBuilder.Entity<RolesModel>()
                .ToTable("tbl_roles")
                .HasKey(r => r.RoleId);
        }
    }
}