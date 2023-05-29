using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniETicaret.Application.Abstractions.Token;

namespace MiniETicaret.Infrastructure.Services.Token;

public class TokenHandler: ITokenHandler
{
    readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Application.DTOs.Token CreateAccessToken(int second)
    {
        Application.DTOs.Token token = new ();
        //SecurityKey simektrik anahtarı oluşturuluyor.
        SymmetricSecurityKey securityKey = new(System.Text.Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        //Anahtarın algoritması belirleniyor.
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        //Oluşturulacak token ayarları yapılıyor.
        token.Expiration = DateTime.Now.AddSeconds(second);
        JwtSecurityToken securityToken = new(
            issuer: _configuration["Token:Issuer"],
            audience: _configuration["Token:Audience"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials
        );
        
        //Token oluşturucu sınıfından bir örnek alalım.
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);
        return token;

    }
}