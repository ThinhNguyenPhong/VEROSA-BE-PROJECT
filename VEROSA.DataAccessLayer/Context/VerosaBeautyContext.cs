﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.DataAccessLayer.Context
{
    public class VerosaBeautyContext : DbContext
    {
        public VerosaBeautyContext(DbContextOptions<VerosaBeautyContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BeautyService> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Áp dụng char(36) và GeneratedOnAdd cho tất cả PK Guid
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var pk = entityType.FindPrimaryKey();
                if (pk != null)
                {
                    var guidProp = pk.Properties.FirstOrDefault(p => p.ClrType == typeof(Guid));
                    if (guidProp != null)
                    {
                        guidProp.SetColumnType("char(36)");
                        guidProp.ValueGenerated = ValueGenerated.OnAdd;
                    }
                }
            }

            // Account cấu hình...
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DateOfBirth).IsRequired().HasColumnType("datetime");
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PasswordHash).HasColumnType("text").IsRequired(false);
                entity.Property(e => e.Role).HasConversion<string>().IsRequired();
                entity.Property(e => e.Status).HasConversion<string>().IsRequired();
                entity
                    .Property(e => e.ConfirmationToken)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity
                    .Property(e => e.ConfirmationTokenExpires)
                    .HasColumnType("datetime")
                    .IsRequired(false);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime").ValueGeneratedOnAdd();
                entity
                    .Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // BlogPost thêm ImageUrl
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.Property(b => b.Type).HasConversion<string>();
                entity
                    .Property(b => b.ImageUrl)
                    .HasColumnType("text")
                    .HasMaxLength(500)
                    .IsRequired(false);
            });

            // BeautyService (Services) thêm ImageUrl
            modelBuilder.Entity<BeautyService>(entity =>
            {
                entity.ToTable("Services");
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Description).IsRequired(false);
                entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
                entity.Property(s => s.CreatedAt).HasColumnType("datetime");
                entity
                    .Property(s => s.ImageUrl)
                    .HasColumnType("text")
                    .HasMaxLength(500)
                    .IsRequired(false);
            });

            // Product thêm ImageUrl
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).IsRequired(false);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.CreatedAt).HasColumnType("datetime");
                entity
                    .Property(p => p.ImageUrl)
                    .HasColumnType("text")
                    .HasMaxLength(500)
                    .IsRequired(false);
            });

            // Cấu hình Payment, DiscountCode, SupportTicket, Appointment...
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Method).HasConversion<string>();
                entity.Property(p => p.Status).HasConversion<string>();
            });
            modelBuilder.Entity<DiscountCode>().Property(d => d.Type).HasConversion<string>();
            modelBuilder.Entity<SupportTicket>().Property(s => s.Status).HasConversion<string>();
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity
                    .HasOne(a => a.Consultant)
                    .WithMany(u => u.ConsultantAppointments)
                    .HasForeignKey(a => a.ConsultantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(a => a.Customer)
                    .WithMany(u => u.CustomerAppointments)
                    .HasForeignKey(a => a.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default
        )
        {
            var now = DateTime.UtcNow;
            // Tự động set CreatedAt, UpdatedAt cho Account
            foreach (var entry in ChangeTracker.Entries<Account>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public async Task<Account> GetByConfirmationTokenAsync(string token) =>
            await Accounts.FirstOrDefaultAsync(a =>
                a.ConfirmationToken == token
                && a.ConfirmationTokenExpires.HasValue
                && a.ConfirmationTokenExpires.Value > DateTime.UtcNow
            );
    }
}
