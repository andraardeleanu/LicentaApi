namespace Api2.ApiModels
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cui { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public CompanyDTO(int id, string name, string cui, string author, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            Cui = cui;
            Author = author;
            DateCreated = dateCreated;
        }
    }
}
