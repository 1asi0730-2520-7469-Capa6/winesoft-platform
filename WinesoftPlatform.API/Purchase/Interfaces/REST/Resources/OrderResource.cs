namespace WinesoftPlatform.API.Purchase.Interfaces.REST.Resources;

public record OrderResource(
    int Id,
    int ProductId,
    string Supplier,
    int Quantity,
    string Status,
    DateTimeOffset? CreatedDate
);