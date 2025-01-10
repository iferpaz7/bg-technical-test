using BG.Application.DTOs.User;
using Common.Utils.Security.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BG.Application.Services;

public class AccountService(ApplicationDbContext context, IConfiguration configuration, ISensitiveDataEncryptionService encryptionService, IMapper mapper) : IAccountService
{
    private string passphrase = configuration["DataProtection:ProtectorKey"];
    public async Task<ApiResponse> LoginAsync(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
            x.Username == loginDto.Username);

        var decryptedPassword =
            await encryptionService.DecryptFromBytesAsync(passphrase, user.Password);

        return loginDto.Password != decryptedPassword
            ? new ApiResponse { Code = "0", Message = "Contraseña incorrecta." }
            : new ApiResponse { Code = "1", Message = "Login Correcto", Payload = mapper.Map<UserDto>(user) };
    }
}
