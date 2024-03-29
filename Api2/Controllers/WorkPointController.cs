using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class WorkPointController : ControllerBase
    {
        private readonly IGenericService<WorkPoint> _workpointService;
        private readonly IGenericService<Company> _companyService;

        public WorkPointController(IGenericService<WorkPoint> workpointService, IGenericService<Company> companyService)
        {
            _workpointService = workpointService;
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize]
        [Route("getWorkpoints")]
        public async Task<IActionResult> GetWorkpointAsync()
        {
            var workpoints = await _workpointService.ListAsync();
            var dtoList = workpoints.Select(x => new WorkPointDTO(x.Id, x.Name, x.Address, x.Author, x.CompanyId));

            return new JsonResult(dtoList);
        }

        [HttpPost]
        [Authorize]
        [Route("addWorkpoint")]
        public async Task<IActionResult> AddWorkpointAsync([FromBody] WorkpointRequest workpointRequest)
        {
            var existingWorkpoint = await _workpointService.WhereAsync(x => x.Name == workpointRequest.Name);
            if (existingWorkpoint != null && existingWorkpoint.Any()) return BadRequest("Exista deja o un punct de lucru cu acest nume.");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "admin";
            var workpointEntity = workpointRequest.ToWorkpointEntity(username);
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

        [HttpGet]
        //[Authorize]
        [Route("getWorkpointByName/{name}")]
        public async Task<IActionResult> GetWorkpointByNameAsync(string name)
        {
            var workpoint = await _workpointService.WhereAsync(x => x.Name.Contains(name));

            return new JsonResult(workpoint);
        }
    }
}
