namespace Api2.Requests
{
    public class OrderFilterRequest
    {
        public Guid? OrderNo { get; set; }
        public string? CreatedBy { get; set; }
        public int? WorkPointId { get; set; }
        public string? Status { get; set; }
    }
}
