using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Bill : BaseEntity
    {
        public DateTime Date {  get; set; }
        public string OrderNo { get; set; }               
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
