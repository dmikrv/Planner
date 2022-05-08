using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PlannerAPI.Entities;
using Action = PlannerAPI.Entities.Action;

namespace PlannerAPI
{
    public partial class PlannerContext : DbContext
    {
        public PlannerContext()
        {
        }

        public PlannerContext(DbContextOptions<PlannerContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Action> Actions { get; set; } = null!;
        public virtual DbSet<ActionState> ActionStates { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Color> Colors { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Energy> Energies { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectState> ProjectStates { get; set; } = null!;
        public virtual DbSet<ScheduledAction> ScheduledActions { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<TrashAction> TrashActions { get; set; } = null!;
        public virtual DbSet<WaitingAction> WaitingActions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Email).HasMaxLength(200);
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("Action");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.Notes).HasMaxLength(4000);

                entity.Property(e => e.Text).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Action_AccountId_Account_Id");

                entity.HasOne(d => d.Energy)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.EnergyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Action_EnergyId_Energy_Id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Action_ProjectId_Project_Id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Action_StateId_ActionState_Id");

                entity.HasMany(d => d.Areas)
                    .WithMany(p => p.Actions);
                
                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Actions);
                
                entity.HasMany(d => d.Contacts)
                    .WithMany(p => p.Actions);
            });

            modelBuilder.Entity<ActionState>(entity =>
            {
                entity.ToTable("ActionState");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Area_AccountId_Account_Id");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Area_ColorId_Color_Id");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Contact_AccountId_Account_Id");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Contact_ColorId_Color_Id");
            });

            modelBuilder.Entity<Energy>(entity =>
            {
                entity.ToTable("Energy");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Notes).HasMaxLength(4000);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Project_AccountId_Account_Id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Project_StateId_ProjectState_Id");
            });

            modelBuilder.Entity<ProjectState>(entity =>
            {
                entity.ToTable("ProjectState");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<ScheduledAction>(entity =>
            {
                entity.HasKey(e => e.ActionId)
                    .HasName("PK__Schedule__FFE3F4D94A152C94");

                entity.ToTable("ScheduledAction");

                entity.Property(e => e.ActionId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Tag_AccountId_Account_Id");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Tag_ColorId_Color_Id");
            });

            modelBuilder.Entity<TrashAction>(entity =>
            {
                entity.ToTable("TrashAction");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TrashActions)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("TrashAction_AccountId_Account_Id");
            });

            modelBuilder.Entity<WaitingAction>(entity =>
            {
                entity.HasKey(e => e.ActionId)
                    .HasName("PK__WaitingA__FFE3F4D98E9FADE1");

                entity.ToTable("WaitingAction");

                entity.Property(e => e.ActionId).ValueGeneratedNever();

                entity.HasOne(d => d.Action)
                    .WithOne(p => p.WaitingAction)
                    .HasForeignKey<WaitingAction>(d => d.ActionId)
                    .HasConstraintName("WaitingAction_ActionId_Action_Id");
            });

            var actionStates = new[]
            {
                new ActionState {Id = 1},
                new ActionState {Id = 2},
                new ActionState {Id = 3},
                new ActionState {Id = 4},
                new ActionState {Id = 5},
            };
            modelBuilder.Entity<ActionState>().HasData(actionStates);

            var colors = new[]
            {
                new Color {Id = 1},
                new Color {Id = 2},
                new Color {Id = 3},
                new Color {Id = 4},
                new Color {Id = 5},
                new Color {Id = 6},
            };
            modelBuilder.Entity<Color>().HasData(colors);

            var energies = new[]
            {
                new Energy {Id = 1},
                new Energy {Id = 2},
                new Energy {Id = 3},
            };
            modelBuilder.Entity<Energy>().HasData(energies);
            
            var projectStates = new[]
            {
                new ProjectState {Id = 1},
                new ProjectState {Id = 2},
                new ProjectState {Id = 3},
            };
            modelBuilder.Entity<ProjectState>().HasData(projectStates);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
