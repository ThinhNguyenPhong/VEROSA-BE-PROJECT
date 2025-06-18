using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class BlogPost
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public PostType Type { get; set; }
        public Guid AuthorId { get; set; }
        public Account Author { get; set; }

        public DateTime PublishedAt { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}
