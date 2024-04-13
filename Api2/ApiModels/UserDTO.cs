using Core.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;

namespace Api2.ApiModels
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Company> Companies { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public string Email { get; set; }
        public UserDTO(string id, string firstName, string lastName, IList<Company> companies, string username, IList<string> role, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Companies = companies;
            Username = username;
            Roles = role;
            Email = email;
        }
    }
}
