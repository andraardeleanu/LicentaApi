namespace Api2.ApiModels
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public string Author { get; set; }
        public int WorkPointId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated {  get; set; }
        public string Status { get; set; }
        public OrderDTO(int id, Guid orderNo, string author, DateTime dateCreated, int workPointId, decimal totalPrice, string status)
        {
            Id = id;
            OrderNo = orderNo;
            Author = author;
            DateCreated = dateCreated;
            WorkPointId = workPointId;
            TotalPrice = totalPrice;
            Status = status;
        }
    }
}
