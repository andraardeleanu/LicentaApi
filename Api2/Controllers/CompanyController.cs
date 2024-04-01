﻿using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Constants;
using Core.Entities;
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
        //[Authorize]
        [Route("getCompanies")]
        public async Task<IActionResult> GetCompaniesAsync([FromQuery] NameFilterRequest companyFilterRequest)
        {
            var companies = await _companyService.ListAsync();

            if (companyFilterRequest.Name != null)
            {
                companies = companies.FindAll(x => x.Name.Contains(companyFilterRequest.Name));
            }
            var dtoList = companies.Select(x => new CompanyDTO(x.Id, x.Name, x.Cui, x.Author, x.DateCreated, x.DateUpdated));

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

        [HttpGet]
        //[Authorize]
        [Route("getCompanyByName/{name}")]
        public async Task<IActionResult> GetCompanyByNameAsync(string name)
        {
            var company = await _companyService.WhereAsync(x => x.Name.Contains(name));

            return new JsonResult(company);
        }

        [HttpPost]
        //[Authorize]
        [Route("addCompany")]
        public async Task<IActionResult> AddCompanyAsync([FromBody] CompanyRequest companyRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            var existingCompany = await _companyService.WhereAsync(x => x.Cui == companyRequest.Cui);
            if (existingCompany != null && existingCompany.Any()) return BadRequest(ErrorMessages.ExistingCui);

            var companyEntity = companyRequest.ToCompanyEntity(user.Id, username);
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
