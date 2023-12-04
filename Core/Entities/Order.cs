using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public int CompanyId { get; set; }
        public int WorkPointId { get; set; }
        public string Status { get; set; }
        public int BillId { get; set; }
        public Enums.FileType FileType { get; set; }
        public virtual List<Product> Products { get; set; }

    }
}
