using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BG.Application.DTOs.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

    public string Create(UserDto appUser)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Name, appUser.Username ?? ""),
            new(JwtRegisteredClaimNames.Sub, Convert.ToString(appUser.Id)),
            new(ClaimTypes.NameIdentifier, Convert.ToString(appUser.Id))
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