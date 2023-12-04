namespace Api2.ApiModels
{
    public class CompanyDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cui { get; set; }
        public CompanyDTO(string id, string name, string cui)
        {
            Id = id;
            Name = name;
            Cui = cui;
        }
    }
}
