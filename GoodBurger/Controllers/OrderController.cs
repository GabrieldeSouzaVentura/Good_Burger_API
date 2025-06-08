using GoodBurger.DTOs;
using GoodBurger.DTOs.OrderResponseDto;
using GoodBurger.Filters;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IUnitOfWork unitOfWork, ILogger<OrderController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [Authorize]
    [HttpGet("MyOrder")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrder()
    {
        var order = await _unitOfWork.OrderRepository.GetOrders();

        if (order == null || !order.Any()) return BadRequest("There are no orders"); _logger.LogInformation("----- Get MyOrder BadRequest-----");

        _logger.LogInformation("----- Get MyOrder -----");
        var result = order.Select(o => new OrderResponseDto
        {
            OrderId = o.OrderId,
            Burger = new BurgerDto
            {
                BurgerId = o.Burger?.BurgerId ?? 0,
                BurgerName = o.Burger?.Name ?? "Invalid",
                BurgerPrice = o.Burger?.Price ?? 0
            },
            Extras = o.Extras.Select(e => new ExtraDto
            {
                ExtraId = e.ExtraId,
                ExtraName = e.Name,
                ExtraPrice = e.Price
            }).ToList(),
            Total = o.Total,
            Discount = o.Discount
        });

        return Ok(result);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPost("CreateOrder/")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<Order>> Post([FromBody] MyOrderDto dto)
    {
        if (dto == null || dto.BurgerId <= 0 || dto.ExtrasIds == null || dto.ExtrasIds.Count == 0 || dto.ExtrasIds.Count > 2)
        {
            _logger.LogInformation("----- Post CreateOrder BadRequest -----");
            return BadRequest("Values invalid");
        }

        _logger.LogInformation("----- Post CreateOrder -----");
        var newOrder = await _unitOfWork.OrderRepository.CreateOrder(dto);

        return Ok(newOrder);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPut("UpdateOrder/{Id:int:min(1)}")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<Order>> Update(int Id, [FromBody] MyOrderDto dto)
    {
        var order = await _unitOfWork.OrderRepository.GetAsync(o => o.OrderId == Id);
        if (order is null) return BadRequest("Update invalid"); _logger.LogInformation($"----- Put UpdateOrder BadRequest {Id} -----");

        _logger.LogInformation($"----- Put UpdateOrder {Id} -----");
        var orderUpdate = await _unitOfWork.OrderRepository.OrderUpdate(Id, dto);

        _unitOfWork.CommitAsync();
        return Ok(orderUpdate);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpDelete("DeleteOrder/{Id:int:min(1)}")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<Order>> Delete(int Id)
    {
        var order = await _unitOfWork.OrderRepository.GetAsync(o => o.OrderId == Id, include: o => o.Include(o => o.Extras));

        if (order is null) return BadRequest("Order invalid"); _logger.LogInformation($"----- Delete DeleteOrder BadRequest {Id} -----");

        _logger.LogInformation($"----- Delete deleteOrder {Id} -----");
        var deleteOrder = _unitOfWork.OrderRepository.Delete(order);
        await _unitOfWork.CommitAsync();

        return Ok(deleteOrder);
    }
}
