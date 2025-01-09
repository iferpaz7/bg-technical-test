using BG.Application.DTOs.User;

namespace BG.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string Create(UserDto userDto);
}