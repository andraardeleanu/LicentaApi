using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Bills
    {
        public DateTime Date {  get; set; }
        public string OrderNo { get; set; }
        public int CompanyId { get; set; }
        public int WorkPointId { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }

    }
}
