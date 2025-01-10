using BG.API.Extensions;
using BG.Application.DTOs.User;
using BG.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BG.API.Controllers;

public class UserController(IUserService userService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] UserFilterDto userFilterDto)
    {
        userFilterDto.UserId = User.GetUserId();
        return Ok(await userService.GetAsync(userFilterDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await userService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateUserDto createUserDto)
    {
        return Ok(await userService.AddAsync(createUserDto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        updateUserDto.UserId = User.GetUserId();
        return Ok(await userService.UpdateAsync(id, updateUserDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        return Ok(await userService.DeleteAsync(User.GetUserId(), id));
    }
}