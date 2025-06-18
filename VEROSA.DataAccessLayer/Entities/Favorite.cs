using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Favorite
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Account Customer { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
