using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.Common.Models.Request
{
    public class BlogPostRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public PostType Type { get; set; }
        public Guid AuthorId { get; set; }
    }
}
