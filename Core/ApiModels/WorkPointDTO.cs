namespace Core.ApiModels
{
    public class WorkPointDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Author { get; set; }
        public int CompanyId { get; set; }
        public WorkPointDTO(int id, string name, string address, string author, int companyId)
        {
            Id = id;
            Name = name;
            Address = address;
            Author = author;
            CompanyId = companyId;
        }
    }
}
