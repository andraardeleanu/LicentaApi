using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class CompanyController : ControllerBase
    {
        private readonly IGenericService<Company> _companyService;

        public CompanyController(IGenericService<Company> companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize]
        [Route("getCompanies")]
        public async Task<IActionResult> GetCompanyAsync()
        {
            var companies = await _companyService.ListAsync();
            var dtoList = companies.Select(x => new CompanyDTO(x.Id, x.Name, x.Cui, x.Author, x.DateCreated));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Authorize]
        [Route("getCompanyById/{id}")]
        public async Task<IActionResult> GetCompanyAsync(int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            return new JsonResult(company);
        }

        [HttpPost]
        [Authorize]
        [Route("addCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] CompanyRequest companyRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var companyEntity = companyRequest.ToCompanyEntity(username);
            var company = await _companyService.AddAsync(companyEntity);

            return new JsonResult(company);
        }

        [HttpPut]
        [Authorize]
        [Route("updateCompany")]
        public async Task<IActionResult> UpdateCompanyAsync([FromBody] CompanyDTO companyRequest)
        {
            var workpoint = await _companyService.GetByIdAsync(companyRequest.Id);

            workpoint.Name = companyRequest.Name;
            workpoint.Cui = companyRequest.Cui;
            workpoint.DateUpdated = DateTime.UtcNow;

            await _companyService.UpdateAsync(workpoint);

            var entityResult = await _companyService.GetByIdAsync(companyRequest.Id, x => x.Name, x => x.Cui);
            return new JsonResult(entityResult);
        }

        [HttpDelete]
        [Authorize]
        [Route("removeCompany")]
        public async Task<IActionResult> RemoveWorkpointAsync(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            await _companyService.DeleteAsync(company);

            return Ok();
        }
    }
}
