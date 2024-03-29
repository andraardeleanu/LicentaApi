using Api2.Requests;
using Core.Entities;

namespace Api2.Mapping
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
