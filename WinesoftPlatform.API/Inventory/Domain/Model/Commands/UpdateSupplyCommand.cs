namespace WinesoftPlatform.API.Inventory.Domain.Model.Commands;

public record UpdateSupplyCommand(int Id, string SupplyName, int Quantity, string Unit, decimal Price);