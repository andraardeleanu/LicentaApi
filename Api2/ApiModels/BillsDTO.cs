namespace Api2.ApiModels
{
    public class BillsDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid OrderNo { get; set; } 
        public string WorkpointName{ get; set; }
        public string CompanyName { get; set; }
        public decimal TotalPrice {  get; set; }         
        public string Status { get; set; }
        public BillsDTO(int id, string author, DateTime dateCreated, Guid orderNo,string workpointName, string companyName, decimal totalPrice, string status)
        {
            Id = id;
            Author = author;
            DateCreated = dateCreated;
            OrderNo = orderNo;
            WorkpointName = workpointName;
            TotalPrice = totalPrice;
            Status = status;
            CompanyName = companyName;
        }
    }
}
