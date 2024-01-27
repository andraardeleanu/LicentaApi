namespace Api2.ApiModels
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
        public ProductDTO(int id, string name, decimal price, string author)
        {
            Id = id;
            Name = name;
            Price = price;
            Author = Author;
        }
    }
}
