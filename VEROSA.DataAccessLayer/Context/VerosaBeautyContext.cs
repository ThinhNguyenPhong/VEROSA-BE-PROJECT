using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var idProp = entity.FindProperty("Id");
                if (idProp != null && idProp.ClrType == typeof(Guid))
                {
                    // Map Guid → CHAR(36) with DEFAULT (UUID())
                    idProp.SetColumnType("char(36)");
                    idProp.SetDefaultValueSql("(UUID())");
                }
            }

            // Enum → string
            modelBuilder.Entity<Account>().Property(a => a.Role).HasConversion<string>();
            modelBuilder.Entity<Account>().Property(a => a.Status).HasConversion<string>();
            modelBuilder
                .Entity<Appointment>()
                .HasOne(a => a.Consultant)
                .WithMany(u => u.ConsultantAppointments)
                .HasForeignKey(a => a.ConsultantId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>().Property(p => p.Method).HasConversion<string>();
            modelBuilder.Entity<Payment>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<SupportTicket>().Property(s => s.Status).HasConversion<string>();
            modelBuilder.Entity<DiscountCode>().Property(d => d.Type).HasConversion<string>();
            modelBuilder.Entity<BlogPost>().Property(b => b.Type).HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
