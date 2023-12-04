namespace Api2.ApiModels
{
    public class OrdersDTO
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyId { get; set; }
        public string WorkPointId { get; set; }
        public string Status { get; set; }
        public OrdersDTO(string id, string orderNo, DateTime date, string createdBy, string companyId, string workPointId, string status)
        {
            Id = id;
            OrderNo = orderNo;
            Date = date;
            CreatedBy = createdBy;
            CompanyId = companyId;
            WorkPointId = workPointId;
            Status = status;
        }
    }
}
