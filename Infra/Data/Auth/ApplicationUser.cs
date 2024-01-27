using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infra.Data.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
