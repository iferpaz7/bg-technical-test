namespace BG.Application.Services;

public class IdentificationTypeService(ApplicationDbContext context, IMapper mapper) : IIdentificationTypeService
{
    public async Task<ApiResponse> GetAllAsync()
    {
        var identificationTypes = await context.IdentificationTypes.ToListAsync();

        return identificationTypes.Count == 0
            ? new ApiResponse { Code = "0", Message = "No se encontraron los tipos de identificación" }
            : new ApiResponse { Code = "1", Payload = mapper.Map<IEnumerable<IdentificationTypeDto>>(identificationTypes) };
    }
}