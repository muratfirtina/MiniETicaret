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
using MiniETicaret.Application.Features.Commands.Cart.AddItemToCart;
using MiniETicaret.Application.Features.Commands.Cart.RemoveCartItem;
using MiniETicaret.Application.Features.Commands.Cart.UpdateCartItem;
using MiniETicaret.Application.Features.Commands.Cart.UpdateQuantity;
using MiniETicaret.Application.Features.Queries.Cart.GetCartItems;

namespace MiniETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    
    public class CartsController : ControllerBase
    {
        readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Reading, Definition = "Get Cart Items")]
        public async Task<IActionResult> GetCartItems([FromQuery]GetCartItemsQueryRequest getCartItemsQueryRequest)
        {
            List<GetCartItemsQueryResponse> response = await _mediator.Send(getCartItemsQueryRequest);
            return Ok(response);
        }
        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Writing, Definition = "Add Item To Cart")]
        public async Task<IActionResult> AddItemToCart(AddItemToCartCommandRequest addItemToCartCommandRequest)
        {
            AddItemToCartCommandResponse response = await _mediator.Send(addItemToCartCommandRequest);
            return Ok(response);
        }
        
        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Updating, Definition = "update quantity")]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest updateQuantityCommandRequest)
        {
            UpdateQuantityCommandResponse response = await _mediator.Send(updateQuantityCommandRequest);
            return Ok(response);
        }
        [HttpDelete("{CartItemId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Deleting, Definition = "Remove Cart Items")]
        public async Task<IActionResult> RemoveCartItem([FromRoute]RemoveCartItemCommandRequest removeCartItemCommandRequest)
        {
            RemoveCartItemCommandResponse response = await _mediator.Send(removeCartItemCommandRequest);
            return Ok(response);
        }
        [HttpPut("UpdateCartItem")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Carts, ActionType = ActionType.Updating, Definition = "Update Cart Item isChecked")]
        public async Task<IActionResult> UpdateCartItem(UpdateCartItemCommandRequest updateCartItemCommandRequest)
        {
            UpdateCartItemCommandResponse response = await _mediator.Send(updateCartItemCommandRequest);
            return Ok(response);
        }
    }
}
