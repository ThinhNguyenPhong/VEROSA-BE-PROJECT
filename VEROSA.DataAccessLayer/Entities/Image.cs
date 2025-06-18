using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Url { get; set; }
        public string AltText { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? ServiceId { get; set; }
        public BeautyService Service { get; set; }

        public Guid? BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
