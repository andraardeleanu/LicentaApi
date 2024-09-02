namespace Core.Requests
{
    public class OrderFilterRequest
    {
        public int? Id { get; set; }
        public string? OrderNo { get; set; }
        public string? CreatedBy { get; set; }
        public int? WorkPointId { get; set; }
        public string? Status { get; set; }
    }
}
