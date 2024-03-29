using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Stock : BaseEntity
    {        
        public int AvailableStock {  get; set; }
        public int PendingStock { get; set; }
        public int ProductId { get; set; }
    }
}
