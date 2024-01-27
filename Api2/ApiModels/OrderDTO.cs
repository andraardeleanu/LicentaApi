namespace Api2.ApiModels
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public Guid OrderNo { get; set; }
        public DateTime Date { get; set; }
        public int WorkPointId { get; set; }
        public string Status { get; set; }
        public OrderDTO(int id, Guid orderNo, DateTime date, int workPointId, string status)
        {
            Id = id;
            OrderNo = orderNo;
            Date = date;
            WorkPointId = workPointId;
            Status = status;
        }
    }
}
