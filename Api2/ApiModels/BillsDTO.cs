namespace Api2.ApiModels
{
    public class BillsDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string OrderNo { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyId { get; set; }
        public string WorkPointId { get; set; }
        public string TotalPrice { get; set; }
        public string Status { get; set; }
        public BillsDTO(string id, string orderNo, DateTime date, string createdBy, string companyId, string workPointId, string totalPrice, string status)
        {
            Id = id;
            Date = date;
            OrderNo = orderNo;
            CreatedBy = createdBy;
            CompanyId = companyId;
            WorkPointId = workPointId;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}
