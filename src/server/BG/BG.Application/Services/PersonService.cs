using BG.Application.DTOs.Person;
using BG.Infrastructure.Data;
using Common.Utils.Security.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BG.Application.Services;

public class PersonService(
    IAdoRepository adoRepository,
    ApplicationDbContext context,
    IMapper mapper,
    IConfiguration configuration,
    ISensitiveDataEncryptionService encryptionService) : IPersonService
{
    private const string ModuleName = "Config.Person_";

    public async Task<ApiResponse> AddAsync(CreatePersonDto createPersonDto)
    {
        if (await context.Persons.AnyAsync(x => x.IdCard == createPersonDto.IdCard))
        {
            return new ApiResponse { Code = "0", Message = "Persona ya existe, con este número de identificación" };
        }

        if (await context.Users.AnyAsync(x => x.Username == createPersonDto.Username))
        {
            return new ApiResponse { Code = "0", Message = "Persona ya existe, con este usuario" };
        }

        var identificationType = await context.IdentificationTypes.FindAsync(createPersonDto.IdentificationTypeId);

        if (identificationType is null)
            return new ApiResponse { Code = "0", Message = "Tipo de identificación no encontrado." };

        var person = mapper.Map<Person>(createPersonDto);

        person.FullName = AddFullName(createPersonDto.FirstName, createPersonDto.LastName);
        person.Code = CreateCode(createPersonDto.IdCard, identificationType.Code);
        
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        var user = new User { Username = createPersonDto.Username };

        var dataProtectionSettings = configuration.GetSection("DataProtection");

        user.Password =
            await encryptionService.EncryptToBytesAsync(dataProtectionSettings["ProtectorKey"],
                createPersonDto.Password);

        context.Users.Add(user);

        await context.SaveChangesAsync();

        return new ApiResponse { Code = "1", Message = "Persona creada correctamente." };
    }

    public async Task<ApiResponse> DeleteAsync(int userId, int id)
    {
        var person = await context.Persons.FindAsync(id);
        if (person is null)
        {
            return new ApiResponse { Code = "0", Message = "Persona no encontrada." };
        }

        // context.Persons.Remove(person); for complete deletion

        person.Deleted = true;
        person.UserId = userId;
        person.Enabled = false;
        context.Persons.Update(person);
        await context.SaveChangesAsync();
        return new ApiResponse { Code = "1", Message = "Persona Eliminada correctamente." };
    }

    public async Task<ApiResponse> GetAsync(PersonaFilterDto personaFilterDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "person", CustomConverters.SerializeObjectCustom<PersonaFilterDto>(personaFilterDto) }
        };
        var dt = await adoRepository.GetDataTableAsync($"{ModuleName}", parameters);
        return CustomValidators.DataTableIsNull(dt)
            ? new ApiResponse { Code = "0", Message = "Data not found!" }
            : new ApiResponse { Code = "1", Payload = CustomConverters.DataTableToJson(dt) };
    }

    public async Task<ApiResponse> GetByIdAsync(int id)
    {
        var person = await context.Persons.FindAsync(id);
        return person is null
            ? new ApiResponse { Code = "0", Message = "Person not found!" }
            : new ApiResponse { Code = "1", Payload = CustomConverters.SerializeObjectCustom<PersonDto>(person) };
    }

    public async Task<ApiResponse> UpdateAsync(int id, UpdatePersonDto updatePersonDto)
    {
        var person = await context.Persons.FindAsync(id);

        if (person is null)
            return new ApiResponse { Code = "0", Message = "Persona no encontrada." };

        if (id != updatePersonDto.Id)
            return new ApiResponse { Code = "0", Message = "Ids no coinciden." };


        var identificationType = await context.IdentificationTypes.FindAsync(updatePersonDto.IdentificationTypeId);

        if (identificationType is null)
            return new ApiResponse { Code = "0", Message = "Tipo de identificación no encontrado." };

        person.FirstName = updatePersonDto.FirstName;
        person.LastName = updatePersonDto.LastName;
        person.Email = updatePersonDto.Email;
        person.IdCard = updatePersonDto.IdCard;
        person.IdentificationTypeId = updatePersonDto.IdentificationTypeId;
        person.FullName = AddFullName(updatePersonDto.FirstName, updatePersonDto.LastName);
        person.Code = CreateCode(updatePersonDto.IdCard, identificationType.Code);
        person.UserId = updatePersonDto.UserId;
        person.Enabled = updatePersonDto.Enabled;

        context.Persons.Update(person);
        await context.SaveChangesAsync();

        return new ApiResponse { Code = "1", Message = "Persona Actualizada correctamente." };
    }

    private static string AddFullName(string firstName, string lastName)
    {
        return $"{firstName} {lastName}";
    }

    private static string CreateCode(string idCard, string identificationTypeCode)
    {
        return $"{idCard}{identificationTypeCode}";
    }
}