using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Request
{
    public class ReviewRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
