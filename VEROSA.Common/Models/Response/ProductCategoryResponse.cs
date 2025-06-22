using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Response
{
    public class ProductCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
