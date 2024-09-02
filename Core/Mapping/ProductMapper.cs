using Core.Entities;
using Core.Requests;

namespace Core.Mapping
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
            };
        }

    }
}
