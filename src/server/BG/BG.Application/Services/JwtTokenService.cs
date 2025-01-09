using BG.Application.DTOs.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BG.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly int _expiryMinutes;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecurityKey"] ??
                                                               throw new InvalidOperationException()));

        _expiryMinutes = Convert.ToInt32(_configuration["JwtSettings:ExpirationInMinutes"]);
    }

    public string Create(UserDto userDto)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Name, userDto.Username ?? ""),
            new(JwtRegisteredClaimNames.Sub, Convert.ToString(userDto.Id)),
            new(ClaimTypes.NameIdentifier, Convert.ToString(userDto.Id))
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var jwtOptions = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: credentials);


        var token = new JwtSecurityTokenHandler().WriteToken(jwtOptions);
        return token;
    }
}