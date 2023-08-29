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
using MiniETicaret.Application.Features.Commands.Order.CompleteOrder;
using MiniETicaret.Application.Features.Commands.Order.CreateOrder;
using MiniETicaret.Application.Features.Commands.Order.RemoveOrder;
using MiniETicaret.Application.Features.Commands.Order.RemoveOrderItem;
using MiniETicaret.Application.Features.Queries.Order.GetAllOrders;
using MiniETicaret.Application.Features.Queries.Order.GetOrderById;

namespace MiniETicaretAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Admin")]
public class OrdersController : ControllerBase
{
    readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing, Definition = "Create Order")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);
        return Ok(response);
    }
    
    [HttpGet]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get All Orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
    {
        GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order By Id")]
    public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }

    [HttpGet("complete-order/{Id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
    public async Task<IActionResult> CompleteOrder( [FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
    {
        CompleteOrderCommandResponse response = await _mediator.Send(completeOrderCommandRequest);
        return Ok(response);
    }
    
    
    [HttpDelete("remove-order-item/{cartItemId}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Deleting, Definition = "Remove Order Item")]
    public async Task<IActionResult> RemoveOrderItem([FromRoute]RemoveOrderItemCommandRequest removeOrderItemCommandRequest)
    {
        RemoveOrderItemCommandResponse response = await _mediator.Send(removeOrderItemCommandRequest);
    
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Deleting, Definition = "Remove Order")]
    public async Task<IActionResult> RemoveOrder([FromRoute] RemoveOrderCommandRequest removeOrderCommandRequest)
    {
        RemoveOrderCommandResponse response = await _mediator.Send(removeOrderCommandRequest);
        return Ok(response);
    }
}