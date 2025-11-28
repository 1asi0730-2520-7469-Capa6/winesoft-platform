namespace WinesoftPlatform.API.Analytics.Interfaces.REST.Resources;

public record RecentOrderResource(int OrderId, 
    string Status,
    DateTime Date,
    int ProductId,
    int Quantity
);