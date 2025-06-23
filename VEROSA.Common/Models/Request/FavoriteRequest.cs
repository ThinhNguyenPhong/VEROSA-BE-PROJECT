using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Request
{
    public class FavoriteRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
