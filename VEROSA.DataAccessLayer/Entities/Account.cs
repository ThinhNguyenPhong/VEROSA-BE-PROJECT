using System.ComponentModel.DataAnnotations;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        // Cho phép NULL khi mới tạo
        public string? PasswordHash { get; set; }

        [Required]
        public AccountRole Role { get; set; }

        [Required]
        public AccountStatus Status { get; set; }

        public string? ConfirmationToken { get; set; }
        public DateTime? ConfirmationTokenExpires { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Quan hệ
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Appointment> CustomerAppointments { get; set; }
        public ICollection<Appointment> ConsultantAppointments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<SupportTicket> SupportTickets { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
