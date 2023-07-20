using MiniETicaret.Application.DTOs.Configuration;

namespace MiniETicaret.Application.Abstractions.Services.Configurations;

public interface IApplicationService //endpoint-authorize service
{
    List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
}