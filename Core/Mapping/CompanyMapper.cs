using Core.Entities;
using Core.Requests;

namespace Core.Mapping
{
    public static class CompanyMapper
    {
        public static Company ToCompanyEntity(this CompanyRequest request, string createdBy, string user)
        {
            return new Company()
            {
                Name = request.Name,
                Cui = request.Cui,
                CreatedBy = createdBy,
                Author = user,
                DateCreated = DateTime.Now,
            };
        }
    }
}
