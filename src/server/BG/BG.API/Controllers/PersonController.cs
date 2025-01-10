using BG.API.Extensions;
using BG.Application.DTOs.Person;
using BG.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BG.API.Controllers;

public class PersonController(IPersonService personService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PersonaFilterDto personaFilterDto)
    {
        personaFilterDto.UserId = User.GetUserId();
        return Ok(await personService.GetAsync(personaFilterDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await personService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreatePersonDto createPersonDto)
    {
        createPersonDto.UserId = User.GetUserId();
        return Ok(await personService.AddAsync(createPersonDto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdatePersonDto updatePersonDto)
    {
        updatePersonDto.UserId = User.GetUserId();
        return Ok(await personService.UpdateAsync(id, updatePersonDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        return Ok(await personService.DeleteAsync(User.GetUserId(), id));
    }
}