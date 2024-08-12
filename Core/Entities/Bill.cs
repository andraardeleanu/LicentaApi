using Core.Common;

namespace Core.Entities
{
    public class Bill : BaseEntity
    {
        public new int Id { get; set; }
        public required string CreatedBy { get; set; }
        public Guid OrderNo { get; set; }
        public required string WorkpointName { get; set; }
        public required string CompanyName { get; set; }
        public decimal TotalPrice { get; set; }
        public required string Status { get; set; }
    }
}
