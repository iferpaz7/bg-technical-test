using BG.API.Filters;
using BG.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BG.API.Controllers;

public class IdentificationTypeController(IIdentificationTypeService identificationTypeService) : BaseApiController
{
    [AllowAnonymous]
    [ValidateClientId("app-bg-tech-test")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await identificationTypeService.GetAllAsync());
    }
}
