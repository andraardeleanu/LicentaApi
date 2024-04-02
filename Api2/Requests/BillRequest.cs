namespace Api2.Requests
{
    public class BillRequest
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string OrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyName { get; set; }
        public string WorkpointName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
