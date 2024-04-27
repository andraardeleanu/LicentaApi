using Api2.Requests;
using Core.Entities;

namespace Api2.ApiModels
{
    public class BillsDTO
    {
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid OrderNo { get; set; } 
        public string WorkpointName{ get; set; }
        public string CompanyName { get; set; }
        public decimal TotalPrice {  get; set; }         
        public string Status { get; set; }
        public BillsDTO(string author, DateTime dateCreated, Guid orderNo,string workpointName, string companyName, decimal totalPrice, string status)
        {
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
