namespace Api2.ApiModels
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public ProductDTO(string id, string name, string price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
