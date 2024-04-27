using Core.Common;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        public Guid OrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public Enums.OrderType OrderType { get; set; }
        public int WorkPointId { get; set; }
        public virtual ICollection<OrderProduct> OrderProduct { get; set; } = new List<OrderProduct>();
    }
}
