using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Planner.Data.Entities;
using Action = Planner.Data.Entities.Action;

namespace Planner.Data
{
    public partial class PlannerContext : IdentityDbContext<Account>
    {
        public PlannerContext(DbContextOptions<PlannerContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
        
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Action> Actions { get; set; } = null!;
        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<TrashAction> TrashActions { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("Actions");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DueDate).HasColumnType("date");
                entity.Property(e => e.ScheduledDate).HasColumnType("date");
                entity.Property(e => e.Notes);
                entity.Property(e => e.Text);
                entity.Property(e => e.Energy);
                entity.Property(e => e.State);
                entity.Property(e => e.IsDone);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.WaitingContact)
                    .WithMany(p => p.WaitingActions)
                    .HasForeignKey(d => d.WaitingContactId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Actions)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(d => d.Areas)
                    .WithMany(p => p.Actions);
                
                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Actions);
                
                entity.HasMany(d => d.Contacts)
                    .WithMany(p => p.Actions);
            });
            
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Areas");
                entity.Property(e => e.Name);
                entity.Property(e => e.Color);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");

                entity.Property(e => e.Name);
                entity.Property(e => e.Color);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DueDate).HasColumnType("date");
                entity.Property(e => e.Name);
                entity.Property(e => e.Notes);
                // entity.Property(e => e.State);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.Property(e => e.Name);
                entity.Property(e => e.Color);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TrashAction>(entity =>
            {
                entity.ToTable("TrashActions");
                entity.Property(e => e.Name);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TrashActions)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(d => d.AccountId);
            });

            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
