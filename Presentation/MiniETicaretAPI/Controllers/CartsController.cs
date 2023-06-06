using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Features.Commands.Cart.AddItemToCart;
using MiniETicaret.Application.Features.Commands.Cart.RemoveCartItem;
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
        public async Task<IActionResult> GetCartItems([FromQuery]GetCartItemsQueryRequest getCartItemsQueryRequest)
        {
            List<GetCartItemsQueryResponse> response = await _mediator.Send(new GetCartItemsQueryRequest());
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddItemToCart(AddItemToCartCommandRequest addItemToCartCommandRequest)
        {
            AddItemToCartCommandResponse response = await _mediator.Send(addItemToCartCommandRequest);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest updateQuantityCommandRequest)
        {
            UpdateQuantityCommandResponse response = await _mediator.Send(updateQuantityCommandRequest);
            return Ok(response);
        }
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem([FromRoute]RemoveCartItemCommandRequest removeCartItemCommandRequest)
        {
            RemoveCartItemCommandResponse response = await _mediator.Send(removeCartItemCommandRequest);
            return Ok(response);
        }
    }
}