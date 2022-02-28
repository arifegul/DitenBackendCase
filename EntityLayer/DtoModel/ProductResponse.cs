using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DtoModel
{
    public class ProductResponse
    {
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public bool DeleteStatus { get; set; } = false;
    }
}
