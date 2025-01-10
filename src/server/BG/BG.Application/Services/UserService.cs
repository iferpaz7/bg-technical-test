using BG.Application.DTOs.User;
using Common.Utils.Security.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BG.Application.Services;

public class UserService(
    ApplicationDbContext context,
    IMapper mapper,
    IConfiguration configuration,
    ISensitiveDataEncryptionService encryptionService) : IUserService
{
    private string passphrase = configuration["DataProtection:ProtectorKey"];
    public async Task<ApiResponse> AddAsync(CreateUserDto createUserDto)
    {
        if (await context.Users.AnyAsync(x => x.Username == createUserDto.Username))
        {
            return new ApiResponse { Code = "0", Message = "Usuario ya existe" };
        }

        var user = new User
        {
            Username = createUserDto.Username,
        };


        user.Password =
            await encryptionService.EncryptToBytesAsync(passphrase, createUserDto.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return new ApiResponse
        { Code = "1", Message = "Usuario creado correctamente." };
    }

    public async Task<ApiResponse> DeleteAsync(int userId, int id)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null)
        {
            return new ApiResponse { Code = "0", Message = "El usuario no existe." };
        }

        //user won't be deleted completely from db
        user.Enabled = false;
        user.Deleted = true;
        user.UserId = userId;
        context.Users.Update(user);

        // context.Users.Remove(user); for complete deletion

        await context.SaveChangesAsync();
        return new ApiResponse { Code = "1", Message = "Usuario eliminado correctamente." };
    }

    public async Task<ApiResponse> GetAsync(UserFilterDto userFilterDto)
    {
        var query = from user in context.Users
                    select new
                    {
                        user,
                    };

        if (!string.IsNullOrEmpty(userFilterDto.TextSearch))
            query = query.Where(x =>
                x.user.Username.Contains(userFilterDto.TextSearch)
            );

        var totalRecords = await query.CountAsync();

        var users = await query
            .Select(x => new
            {
                x.user,
            })
            .Skip((userFilterDto.PageIndex - 1) * userFilterDto.PageSize)
            .Take(userFilterDto.PageSize)
            .ToListAsync();

        if (users.Count == 0)
        {
            return new ApiResponse { Code = "0", Message = "Data not found!", Payload = new { TotalRecords = 0 } };
        }

        return new ApiResponse
        {
            Code = "1",
            Payload = new
            {
                TotalRecords = totalRecords,
                Users = users
            }
        };
    }
    public async Task<ApiResponse> GetByIdAsync(int id)
    {
        var user = await context.Users.FindAsync(id);

        return user is null
            ? new ApiResponse { Code = "0", Message = "Usuario no encontrado!" }
            : new ApiResponse { Code = "1", Payload = mapper.Map<UserDto>(user) };
    }

    public async Task<ApiResponse> UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await context.Users.FindAsync(id);

        if (user is null)
        {
            return new ApiResponse { Code = "0", Message = "El usuario no existe." };
        }

        user.Username = updateUserDto.Username;
        user.Password = await encryptionService.EncryptToBytesAsync(passphrase,
            updateUserDto.Password);
        user.Enabled = updateUserDto.Enabled;
        user.UserId = updateUserDto.UserId;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return new ApiResponse { Code = "1", Message = "Usuario actualizado correctamente." };
    }
}