using MiniETicaret.Domain.Entities.Identity;

namespace MiniETicaret.Application.Abstractions.Token;

public interface ITokenHandler
{
    DTOs.Token CreateAccessToken(int second, AppUser appUser);
    string CreateRefreshToken();

}