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
using System.ComponentModel.Design;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class WorkPointController : ControllerBase
    {
        private readonly IGenericService<WorkPoint> _workpointService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericService<Order> _orderService;

        public WorkPointController(IGenericService<WorkPoint> workpointService, 
            UserManager<ApplicationUser> userManager, 
            IGenericService<Order> orderService)
        {
            _workpointService = workpointService;
            _userManager = userManager;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        [Route("getWorkpoints")]
        public async Task<IActionResult> GetWorkpointAsync([FromQuery] NameFilterRequest workpointFilterRequest)
        {
            var workpoints = await _workpointService.ListAsync();

            if (workpointFilterRequest.Name != null)
            {
                workpoints = workpoints.FindAll(x => x.Name.Contains(workpointFilterRequest.Name));
            }
            var dtoList = workpoints.Select(x => new WorkPointDTO(x.Id, x.Name, x.Address, x.Author, x.CompanyId));

            return new JsonResult(dtoList);
        }

        [HttpGet]
        //[Authorize]
        [Route("getWorkpointBy/{id}")]
        public async Task<IActionResult> GetWorkpointByIdAsync(int id)
        {
            var workpoint = await _workpointService.GetByIdAsync(id);

            return new JsonResult(workpoint);
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
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
           
            if (string.IsNullOrWhiteSpace(workpointRequest.Name) || string.IsNullOrWhiteSpace(workpointRequest.Address))
            {
                return BadRequest(new Result(ErrorMessages.AllFieldsAreMandatory));
            }
            else
            {
                var existingWorkpoint = await _workpointService.WhereAsync(x => x.Name == workpointRequest.Name && x.Address == workpointRequest.Address);
                if (existingWorkpoint != null && existingWorkpoint.Any())
                {
                    return BadRequest(new Result(ErrorMessages.ExistingWorkpoint));
                }
                else
                {
                    workpointRequest.CompanyId = user.CompanyId;
                    var workpointEntity = workpointRequest.ToWorkpointEntity(user.Id, username);
                    var workpoint = await _workpointService.AddAsync(workpointEntity);
                    return Ok(new Result());
                }
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("updateWorkpoint")]
        public async Task<IActionResult> UpdateCompanyAsync([FromBody] UpdateWorkpointRequest workpointRequest)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            var workpoint = await _workpointService.GetByIdAsync(workpointRequest.Id);

            if (string.IsNullOrWhiteSpace(workpointRequest.Name) || string.IsNullOrWhiteSpace(workpointRequest.Address))
            {
                return BadRequest(new Result(ErrorMessages.AllFieldsAreMandatory));
            }

            else
            {
                var existingCompany = await _workpointService.WhereAsync(x => x.Name == workpointRequest.Name && x.Address == workpointRequest.Address);
                if (existingCompany != null && existingCompany.Any())
                {
                    return BadRequest(new Result(ErrorMessages.ExistingWorkpoint));
                }
                else
                {
                    workpoint.Name = workpointRequest.Name;
                    workpoint.Address = workpointRequest.Address;
                    workpoint.DateUpdated = DateTime.UtcNow;

                    await _workpointService.UpdateAsync(workpoint);

                    return Ok(new Result());
                }
            }           
        }

        [HttpPost]
        [Authorize]
        [Route("removeWorkpoint")]
        public async Task<IActionResult> RemoveWorkpointAsync([FromBody] int id)
        {
            var workpointsOrders = await _orderService.WhereAsync(x => x.WorkPointId == id);
            if(workpointsOrders != null && workpointsOrders.Any())
            {
                return BadRequest(new Result(ErrorMessages.CannotDeleteWorkpoint));
            }
            else
            {
                var workpoint = await _workpointService.GetByIdAsync(id);
                await _workpointService.DeleteAsync(workpoint);

                return Ok();
            }            
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
