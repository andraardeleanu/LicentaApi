using System.Data;

namespace Api2.ApiModels
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public UserDTO(string id, string firstName, string lastName, int companyId, string username, IList<string> role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            CompanyId = companyId;
            Username = username;
            Roles = role;
        }
    }
}
