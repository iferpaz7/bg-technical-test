using BG.Application.DTOs;
using BG.Application.DTOs.Person;
using BG.Application.DTOs.User;
using BG.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BG.API.Controllers;

public class AccountController(IUserService userService, IPersonService personService, IJwtTokenService jwtTokenService)
    : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var response = await userService.LoginAsync(loginDto);

        if (response.Code == "1")
            response.Payload = new
            {
                loginDto.Username,
                Token = jwtTokenService.Create(new UserDto { Username = loginDto.Username })
            };

        return Ok(response);
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreatePersonDto createPersonDto)
    {
        var response = await personService.AddAsync(createPersonDto);

        if (response.Code == "1")
            response.Payload = new
            {
                createPersonDto.Username,
                Token = jwtTokenService.Create(new UserDto
                { Username = createPersonDto.Username }),
            };

        return Ok(response);
    }
}