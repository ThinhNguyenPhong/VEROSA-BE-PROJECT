using System;
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
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Map all Guid PKs → CHAR(36), let EF generate on add
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

            // 2. Account configuration
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Role).HasConversion<string>().IsRequired();

                entity.Property(e => e.Status).HasConversion<string>().IsRequired();

                // Timestamps: values set in SaveChangesAsync override
                entity.Property(e => e.CreatedAt).HasColumnType("datetime").ValueGeneratedOnAdd();

                entity
                    .Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // 3. Enum → string for other entities
            modelBuilder.Entity<BlogPost>().Property(b => b.Type).HasConversion<string>();

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Method).HasConversion<string>();
                entity.Property(p => p.Status).HasConversion<string>();
            });

            modelBuilder.Entity<SupportTicket>().Property(s => s.Status).HasConversion<string>();

            modelBuilder.Entity<DiscountCode>().Property(d => d.Type).HasConversion<string>();

            // 4. Appointment relationships
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
    }
}
