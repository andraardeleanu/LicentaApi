using Core.Common;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        public Guid OrderNo { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }
        public string Status { get; set; }        
        public Enums.OrderType FileType { get; set; } // change this prop to order type in db and models
        public int WorkPointId { get; set; }
        public virtual WorkPoint WorkPoint { get; set; }
        public virtual ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }
}
