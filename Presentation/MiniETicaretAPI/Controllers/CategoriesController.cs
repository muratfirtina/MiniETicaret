using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Consts;
using MiniETicaret.Application.CustomAttributes;
using MiniETicaret.Application.Enums;
using MiniETicaret.Application.Features.Commands.Category.CreateCategory;
using MiniETicaret.Application.Features.Commands.Category.RemoveCategory;
using MiniETicaret.Application.Features.Queries.Category.GetAllCategories;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class CategoriesController : ControllerBase
    {
        readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Categories, ActionType = ActionType.Writing,
            Definition = "Create Category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequest createCategoryCommandRequest)
        {
            CreateCategoryCommandResponse response = await _mediator.Send(createCategoryCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Categories, ActionType = ActionType.Reading,
            Definition = "Get All Categories")]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] GetAllCategoriesQueryRequest getAllCategoriesQueryRequest)
        {
            GetAllCategoriesQueryResponse response = await _mediator.Send(getAllCategoriesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Categories, ActionType = ActionType.Deleting,
            Definition = "Delete Category")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] RemoveCategoryCommandRequest deleteCategoryCommandRequest)
        {
            RemoveCategoryCommandResponse response = await _mediator.Send(deleteCategoryCommandRequest);
            return Ok(response);
        }
    }

}
