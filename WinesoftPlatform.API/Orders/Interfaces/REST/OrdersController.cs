using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WinesoftPlatform.API.Orders.Domain.Model.Aggregates;
using WinesoftPlatform.API.Orders.Domain.Repositories;
using WinesoftPlatform.API.Orders.Interfaces.REST.Resources;
using WinesoftPlatform.API.Shared.Domain.Repositories;

namespace WinesoftPlatform.API.Orders.Interfaces.REST;

[ApiController]
[Route("/api/v1/[controller]")]
[SwaggerTag("Create, read, update, and delete orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [SwaggerOperation("Get all orders")]
    public async Task<IEnumerable<OrderResource>> GetAllAsync()
    {
        var orders = await _orderRepository.ListAsync();
        var resources = orders.Select(o => new OrderResource(
            o.Id, o.ProductId, o.Supplier, o.Quantity, o.Status, o.CreatedDate
        ));
        return resources;
    }

    [HttpPost]
    [SwaggerOperation("Create a new order")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateOrderResource resource)
    {
        var order = new Order
        {
            ProductId = resource.ProductId,
            Supplier = resource.Supplier,
            Quantity = resource.Quantity,
            Status = resource.Status
        };
        
        await _orderRepository.AddAsync(order);
        await _unitOfWork.CompleteAsync();
        
        var orderResource = new OrderResource(
            order.Id, order.ProductId, order.Supplier, order.Quantity, order.Status, order.CreatedDate
        );
        
        return Ok(orderResource);
    }
    
    [HttpGet("{id:int}", Name = "GetOrderById")] 
    [SwaggerOperation("Get order by id")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return NotFound();
        
        var resource = new OrderResource(
            order.Id, order.ProductId, order.Supplier, order.Quantity, order.Status, order.CreatedDate 
        );
        return Ok(resource);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation("Update an existing order")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateOrderResource resource)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return NotFound();

        order.ProductId = resource.ProductId;
        order.Supplier = resource.Supplier;
        order.Quantity = resource.Quantity;
        order.Status = resource.Status;

        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();
        
        var updatedResource = new OrderResource(
            order.Id, order.ProductId, order.Supplier, order.Quantity, order.Status, order.CreatedDate
        );
        return Ok(updatedResource);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation("Delete an existing order")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return NotFound();

        _orderRepository.Remove(order);
        await _unitOfWork.CompleteAsync();
        
        return Ok(new { message = $"Order with id {id} deleted successfully." });
    }
}