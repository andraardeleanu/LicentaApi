namespace Api2.Requests
{
    public class OrderRequest
    {
        public Guid OrderNo { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
