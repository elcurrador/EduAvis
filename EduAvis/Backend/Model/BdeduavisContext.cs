using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduAvis.Backend.Model
{
    public partial class BdeduavisContext : DbContext
    {
        public BdeduavisContext() { }

        public BdeduavisContext(DbContextOptions<BdeduavisContext> options)
            : base(options) { }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning Move connection string out of source code for security.
            => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=mysql;database=bdeduavis", ServerVersion.Parse("8.0.40-mysql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.GroupCode).HasName("PRIMARY");
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.Property(e => e.IsSanctioned).HasDefaultValueSql("'0'");
                entity.Property(e => e.RegisteredDatetime).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Reason).WithMany(p => p.Incidents)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("incidents_ibfk_1");

                entity.HasOne(d => d.RegisteredByNavigation).WithMany(p => p.IncidentRegisteredByNavigations)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("incidents_ibfk_4");

                entity.HasOne(d => d.StudentNiaNavigation).WithMany(p => p.Incidents)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("incidents_ibfk_2");

                entity.HasOne(d => d.TeacherDniNavigation).WithMany(p => p.IncidentTeacherDniNavigations)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("incidents_ibfk_3");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission");
                entity.HasKey(e => new { e.RoleId, e.PermissionId });

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_permission_ibfk_1");

                entity.HasOne(e => e.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_permission_ibfk_2");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Nia).HasName("PRIMARY");
                entity.Property(e => e.Nia).ValueGeneratedNever();

                entity.HasOne(d => d.GroupCodeNavigation).WithMany(p => p.Students)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("students_ibfk_1");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Dni).HasName("PRIMARY");

                entity.HasOne(d => d.Role).WithMany(p => p.Teachers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("teachers_ibfk_2");

                entity.HasOne(d => d.TutorGroupNavigation).WithMany(p => p.Teachers)
                    .HasConstraintName("teachers_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
