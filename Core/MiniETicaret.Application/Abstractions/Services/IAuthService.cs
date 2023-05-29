using MiniETicaret.Application.Abstractions.Services.Authentication;

namespace MiniETicaret.Application.Abstractions.Services;

public interface IAuthService : IExternalAuthentication, IInternalAuthentication
{
    
    
}