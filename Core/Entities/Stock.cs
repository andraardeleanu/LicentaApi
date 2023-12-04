using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Stock
    {
        public int ProductId { get; set; }
        public int AvailableStock {  get; set; }
        public int PendingStock { get; set; }
    }
}
