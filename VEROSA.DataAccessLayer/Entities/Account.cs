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

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public AccountRole Role { get; set; }

        [Required]
        public AccountStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
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
