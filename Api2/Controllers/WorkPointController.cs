using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Constants;
using Core.Entities;
using Core.Services.Interfaces;
using Infra.Data.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class WorkPointController : ControllerBase
    {
        private readonly IGenericService<WorkPoint> _workpointService;
        private readonly IGenericService<Company> _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkPointController(IGenericService<WorkPoint> workpointService, IGenericService<Company> companyService, UserManager<ApplicationUser> userManager)
        {
            _workpointService = workpointService;
            _companyService = companyService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("getWorkpoints")]
        public async Task<IActionResult> GetWorkpointAsync([FromQuery] NameFilterRequest workpointFilterRequest)
        {
            var workpoints = await _workpointService.ListAsync();

            if(workpointFilterRequest.Name != null)
            {
                workpoints = workpoints.FindAll(x => x.Name.Contains(workpointFilterRequest.Name));
            }
            var dtoList = workpoints.Select(x => new WorkPointDTO(x.Id, x.Name, x.Address, x.Author, x.CompanyId));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        [Authorize]
        [Route("getWorkpointsByUserId/{userId}")]
        public async Task<IActionResult> GetWorkpointsByUserIdAsync(string userId)
        {
            var userWorkpoints = await _workpointService.WhereAsync(x => x.CreatedBy == userId);
            return Ok(userWorkpoints);
        }

        [HttpPost]
        [Authorize]
        [Route("addWorkpoint")]
        public async Task<IActionResult> AddWorkpointAsync([FromBody] WorkpointRequest workpointRequest)
        {
            var existingWorkpoint = await _workpointService.WhereAsync(x => x.Name == workpointRequest.Name && x.Address == workpointRequest.Address);
            if (existingWorkpoint != null && existingWorkpoint.Any()) return BadRequest(ErrorMessages.ExistingWorkpoint);

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            var workpointEntity = workpointRequest.ToWorkpointEntity(user.Id, username);
            var workpoint = await _workpointService.AddAsync(workpointEntity);

            return new JsonResult(workpoint);
        }

        [HttpPut]
        [Authorize]
        [Route("updateWorkpoint")]
        public async Task<IActionResult> UpdateWorkpointAsync([FromBody] WorkPointDTO workpointRequest)
        {
            var workpoint = await _workpointService.GetByIdAsync(workpointRequest.Id);

            workpoint.Name = workpointRequest.Name;
            workpoint.Address = workpointRequest.Address;
            workpoint.DateUpdated = DateTime.UtcNow;

            await _workpointService.UpdateAsync(workpoint);

            var entityResult = await _workpointService.GetByIdAsync(workpointRequest.Id, x => x.Name, x => x.Address);
            return new JsonResult(entityResult);
        }

        [HttpDelete]
        [Authorize]
        [Route("removeWorkpoint")]
        public async Task<IActionResult> RemoveWorkpointAsync(int id)
        {
            var workpoint = await _workpointService.GetByIdAsync(id);
            await _workpointService.DeleteAsync(workpoint);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("getWorkpointsFromCompany/{companyId}")]
        public async Task<IActionResult> GetWorkpointsFromCompany([FromRoute] int companyId)
        {
            var companyWorkpoints = await _workpointService.WhereAsync(x => x.CompanyId == companyId);
            return Ok(companyWorkpoints);
        }
    }
}
