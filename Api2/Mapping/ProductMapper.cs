using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
{
    public static class ProductMapper
    {
        public static Product ToProductEntity(this ProductRequest request, string user)
        {
            return new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }

    }
}
