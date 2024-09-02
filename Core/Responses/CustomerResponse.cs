using System.Diagnostics.Contracts;

namespace Core.Responses
{
    public class CustomerResponse
    {
        public string CustomerId { get; set; }
        public string Username { get; set; }
        public int CompanyId { get; set; }
    }
}
