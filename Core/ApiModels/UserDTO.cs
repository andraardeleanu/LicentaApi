using Core.Entities;
using System.Data;

namespace Core.ApiModels
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public string Email { get; set; }
        public UserDTO(string id, string firstName, string lastName, int companyId, string companyName, string username, IList<string> role, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            CompanyId = companyId;
            CompanyName = companyName;
            Username = username;
            Roles = role;
            Email = email;
        }
    }
}
