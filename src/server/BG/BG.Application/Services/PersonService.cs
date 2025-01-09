using BG.Application.DTOs.Person;
using Common.Utils.Security.Interfaces;
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

        if (identificationType.Code == "04" && !ValidateRuc(createPersonDto.IdCard.Length))
            return new ApiResponse { Code = "0", Message = "El ruc debe tener 13 digitos" };

        if (identificationType.Code == "05" && !ValidateCedula(createPersonDto.IdCard.Length))
            return new ApiResponse { Code = "0", Message = "La cédula debe tener 10 digitos" };

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
            { "userId", personaFilterDto.UserId },
            { "filters", CustomConverters.SerializeObjectCustom<PersonaFilterDto>(personaFilterDto) }
        };
        var dt = await adoRepository.GetDataTableAsync($"{ModuleName}Get", parameters);
        return CustomValidators.DataTableIsNull(dt)
            ? new ApiResponse { Code = "0", Message = "No se encontraron datos!" }
            : new ApiResponse { Code = "1", Payload = CustomConverters.DataTableToJson(dt) };
    }

    public async Task<ApiResponse> GetByIdAsync(int id)
    {
        var person = await context.Persons.FindAsync(id);

        return person is null
            ? new ApiResponse { Code = "0", Message = "Persona no encontrada!" }
            : new ApiResponse { Code = "1", Payload = mapper.Map<PersonDto>(person) };
    }

    public async Task<ApiResponse> UpdateAsync(int id, UpdatePersonDto updatePersonDto)
    {

        if (!await context.Persons.AnyAsync(x => x.Id == id))
            return new ApiResponse { Code = "0", Message = "Persona no encontrada." };

        if (id != updatePersonDto.Id)
            return new ApiResponse { Code = "0", Message = "Ids no coinciden." };



        var identificationType = await context.IdentificationTypes.FindAsync(updatePersonDto.IdentificationTypeId);

        if (identificationType.Code == "04" && !ValidateRuc(updatePersonDto.IdCard.Length))
            return new ApiResponse { Code = "0", Message = "El ruc debe tener 13 digitos" };

        if (identificationType.Code == "05" && !ValidateCedula(updatePersonDto.IdCard.Length))
            return new ApiResponse { Code = "0", Message = "La cédula debe tener 10 digitos" };


        if (identificationType is null)
            return new ApiResponse { Code = "0", Message = "Tipo de identificación no encontrado." };

        var person = mapper.Map<Person>(updatePersonDto);

        person.FullName = AddFullName(updatePersonDto.FirstName, updatePersonDto.LastName);
        person.Code = CreateCode(updatePersonDto.IdCard, identificationType.Code);

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

    private static bool ValidateCedula(int idCardLength)
    {
        return idCardLength == 10;
    }
    private static bool ValidateRuc(int idCardLength)
    {
        return idCardLength == 13;
    }
}