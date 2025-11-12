namespace WinesoftPlatform.API.Inventory.Interfaces.REST.Resources;

public record UpdateSupplyResource(int Id, string SupplyName, int Quantity, string Unit, decimal Price);