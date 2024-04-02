using Api2.ApiModels;
using Api2.Mapping;
using Api2.Requests;
using Api2.Responses;
using Api2.Services.Interfaces;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Services.Interfaces;
using Infra.Data.Auth;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api2.Controllers
{
    public class BillController : ControllerBase
    {
        private readonly IGenericService<Bill> _billService;
        private readonly IBillGeneratorService _billGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(IBillGeneratorService billGeneratorService, IGenericService<Bill> billService, UserManager<ApplicationUser> userManager)
        {
            _billGeneratorService = billGeneratorService ?? throw new ArgumentNullException(nameof(billGeneratorService));
            _billService = billService;
            _userManager = userManager;
        }

        [HttpPost("/billGenerator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<GenericFileResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateOrderBill([FromBody] BillRequest request)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            var billDto = request.ToBillDTOEntity(user.Id);

            var resultDto = await _billGeneratorService.GenerateOrderBillDocument(billDto);
            
            if (resultDto is null) return BadRequest(ErrorMessages.PdfGenerationFailed);

            var resp = new GenericFileResponse()
            {
                File = resultDto
            };
           
           // var billEntity = request.ToBillEntity(user.Id, username);
            //var bill = await _billService.AddAsync(billEntity);

            return Ok(new Result<GenericFileResponse>(resp));
        }
    }
}
