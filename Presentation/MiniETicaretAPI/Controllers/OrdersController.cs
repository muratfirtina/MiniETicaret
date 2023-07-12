using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETicaret.Application.Features.Commands.Order.CreateOrder;
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
    public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
    {
        GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }
}