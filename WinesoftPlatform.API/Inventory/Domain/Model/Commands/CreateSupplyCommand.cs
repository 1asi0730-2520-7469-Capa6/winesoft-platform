namespace WinesoftPlatform.API.Inventory.Domain.Model.Commands;

public record CreateSupplyCommand(string SupplyName, int Quantity, string Unit, decimal Price);