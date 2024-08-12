using System.Diagnostics.Contracts;

namespace Api2.Responses
{
    public class CustomerResponse
    {
        public string CustomerId { get; set; }
        public string Username { get; set; }
        public int CompanyId { get; set; }
    }
}
