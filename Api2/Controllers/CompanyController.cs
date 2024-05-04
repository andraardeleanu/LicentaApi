using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Services.Interfaces;
using Infra.Data.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class CompanyController : ControllerBase
    {
        private readonly IGenericService<Company> _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyController(IGenericService<Company> companyService, UserManager<ApplicationUser> userManager)
        {
            _companyService = companyService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("getCompanies")]
        public async Task<IActionResult> GetCompaniesAsync([FromQuery] NameFilterRequest companyFilterRequest)
        {
            var companies = await _companyService.ListAsync();

            if (companyFilterRequest.Name != null)
            {
                companies = companies.FindAll(x => x.Name.Contains(companyFilterRequest.Name));
            }

            companies = companies.OrderByDescending(x => x.DateCreated).ToList();

            var dtoList = companies.Select(x => new CompanyDTO(x.Id, x.Name, x.Cui, x.Author, x.DateCreated, x.DateUpdated));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Authorize]
        [Route("getCompanyById")]
        public async Task<IActionResult> GetCompanyAsync([FromQuery] int id)
        {
            try
            {
                var company = await _companyService.GetByIdAsync(id);
                return new JsonResult(company);
            }
            catch (Exception ex)
            {
                return BadRequest(new Result(ErrorMessages.CompanyNotFound));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("addCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] CompanyRequest companyRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == null)
            {
                return BadRequest(new Result(ErrorMessages.NotAuthorized));
            }

            var user = await _userManager.FindByNameAsync(username);

            if (string.IsNullOrWhiteSpace(companyRequest.Name) || string.IsNullOrWhiteSpace(companyRequest.Cui))
            {
                return BadRequest(new Result(ErrorMessages.AllFieldsAreMandatory));
            }
            else
            {
                var existingCompany = await _companyService.WhereAsync(x => x.Name == companyRequest.Name);
                if (existingCompany != null && existingCompany.Any())
                {
                    return BadRequest(new Result(ErrorMessages.ExistingCompanyName));
                }
                else
                {
                    var companyEntity = companyRequest.ToCompanyEntity(user.Id, username);
                    var company = await _companyService.AddAsync(companyEntity);
                    return Ok(new Result());
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("updateCompany")]
        public async Task<IActionResult> UpdateCompanyAsync([FromBody] UpdateCompanyRequest companyRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username!);

            if (string.IsNullOrWhiteSpace(companyRequest.Name))
            {
                return BadRequest(new Result(ErrorMessages.AllFieldsAreMandatory));
            }
            else
            {
                var existingCompany = await _companyService.WhereAsync(x => x.Name == companyRequest.Name);
                if (existingCompany != null && existingCompany.Any())
                {
                    return BadRequest(new Result(ErrorMessages.ExistingCompanyName));
                }
                else
                {
                    var company = await _companyService.GetByIdAsync(companyRequest.Id);

                    company.Name = companyRequest.Name;
                    company.DateUpdated = DateTime.UtcNow;

                    await _companyService.UpdateAsync(company);

                    return Ok(new Result());
                }
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("removeCompany")]
        public async Task<IActionResult> RemoveWorkpointAsync(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            await _companyService.DeleteAsync(company);

            return Ok();
        }
    }
}
