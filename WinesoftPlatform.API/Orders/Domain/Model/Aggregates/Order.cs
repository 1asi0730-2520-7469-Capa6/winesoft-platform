using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace WinesoftPlatform.API.Orders.Domain.Model.Aggregates;

public class Order : IEntityWithCreatedUpdatedDate
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    
    // De EntityFrameworkCore.CreatedUpdatedDate
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}