using Api2.Requests;
using Core.Entities;

namespace Api2.ApiModels
{
    public class BillsDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string OrderNo { get; set; }
        public string CreatedBy { get; set; }       
        public int WorkpointId { get; set; }
        public decimal TotalPrice {  get; set; }         
        public string Status { get; set; }
        public virtual List<Product> Products { get; set; }
        public BillsDTO(string orderNo, DateTime date,  string createdBy, int workpointId, decimal totalPrice, string status, List<Product> products)
        {
            OrderNo = orderNo;
            Date = date;            
            CreatedBy = createdBy;
            WorkpointId = workpointId;
            TotalPrice = totalPrice;
            Status = status;
            Products = products;
        }
    }
}
