namespace Api2.Requests
{
    public class OrderFilterRequest
    {
        public string? OrderNo { get; set; }
        public string? CreatedBy { get; set; }
        public int? WorkPointId { get; set; }
        public string? Status { get; set; }
    }
}
