using MiniETicaret.Application.DTOs.Configuration;

namespace MiniETicaret.Application.Features.Queries.AuthorizeDefinition;

public class GetAuthorizeDefinitionEnpointsQueryResponse
{
    public List<MenuDto>? AuthorizeMenu { get; set; }
}