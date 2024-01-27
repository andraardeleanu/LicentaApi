using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
{
    public static class OrderMapper
    {
        public static Order ToOrderEntity(string user)
        {
            return new Order()
            {                
                Date = DateTime.Now,
                Author = user,
            };
        }
    }
}
