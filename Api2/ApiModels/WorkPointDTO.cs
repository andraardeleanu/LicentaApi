namespace Api2.ApiModels
{
    public class WorkPointDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyId { get; set; }
        public WorkPointDTO(string id, string name, string address, string createdBy, string companyId)
        {
            Id = id;
            Name = name;
            Address = address;
            CreatedBy = createdBy;
            CompanyId = companyId;
        }
    }
}
