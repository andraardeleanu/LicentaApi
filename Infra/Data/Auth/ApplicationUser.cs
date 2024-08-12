using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infra.Data.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int CompanyId { get; set; }
    }
}
