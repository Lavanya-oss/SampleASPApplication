using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.PowerBI.Api.Models;

namespace StudentsProfileApplication.Models;

public partial class StudentsInfoDBContext : DbContext
{
    public StudentsInfoDBContext()
    {
    }

    public StudentsInfoDBContext(DbContextOptions<StudentsInfoDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Address> Address { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=COGNINE-L181\\SQLEXPRESS;Database=StudentsDB;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
        });

        modelBuilder.Entity<Department>().ToTable("Departments");

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasIndex(e => e.CourseId, "IX_Enrollments_CourseID");

            entity.HasIndex(e => e.StudentId, "IX_Enrollments_StudentID");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments).HasForeignKey(d => d.StudentId);
        });


        //modelBuilder.Entity<Student>(entity =>
        //{
        //    entity.Property(e => e.Gpa).HasColumnType("decimal(18, 2)");
        //    entity.Property(e => e.Password)
        //        .HasMaxLength(20)
        //        .IsUnicode(false);
        //    entity.HasOne(s => s.Departments)
        //    .WithOne(d => d.Student)
        //    .HasForeignKey<Student>(s => s.DepartmentID);
        //});

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
        });
        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
        });

        modelBuilder.Entity<TeachingAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId);

            entity.HasIndex(e => e.CourseId, "IX_TeachingAssignments_CourseId");

            entity.HasIndex(e => e.TeacherId, "IX_TeachingAssignments_TeacherId");

            entity.Property(e => e.AssignmentId).HasColumnName("AssignmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Courses).WithMany(p => p.TeachingAssignments).HasForeignKey(d => d.CourseId);

            entity.HasOne(d => d.Teachers).WithMany(p => p.TeachingAssignments).HasForeignKey(d => d.TeacherId);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
