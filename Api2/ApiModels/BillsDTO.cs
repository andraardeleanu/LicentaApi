namespace Api2.ApiModels
{
    public class BillsDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string OrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyName { get; set; }
        public string WorkpointName { get; set; }
        public decimal TotalPrice {  get; set; }         
        public string Status { get; set; }
        public BillsDTO(string id, DateTime date, string orderNo, string createdBy, string companyName, string workpointName, decimal totalPrice, string status)
        {
            Id = id;
            Date = date;
            OrderNo = orderNo;
            CreatedBy = createdBy;
            CompanyName = companyName;
            WorkpointName = workpointName;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}
