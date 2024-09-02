namespace Core.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public int WorkpointId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
