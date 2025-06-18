using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Account Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public Payment Payment { get; set; }
        public Guid? DiscountCodeId { get; set; }
        public DiscountCode DiscountCode { get; set; }
    }
}
