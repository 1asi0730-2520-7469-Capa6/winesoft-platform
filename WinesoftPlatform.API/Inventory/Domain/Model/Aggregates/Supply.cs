using WinesoftPlatform.API.Inventory.Domain.Model.Commands;

namespace WinesoftPlatform.API.Inventory.Domain.Model.Aggregates;

public partial class Supply
{
    public int Id { get; }
    public string SupplyName { get; private set; }
    public int Quantity { get; private set; }
    public string Unit { get; private set; }
    public decimal Price { get; private set; }

    protected Supply()
    {
        SupplyName = string.Empty;
        Quantity = int.MinValue;
        Unit = string.Empty;
        Price = decimal.MinValue;
    }
    
    public Supply(CreateSupplyCommand command)
    {
        SupplyName = command.SupplyName;
        Quantity = command.Quantity;
        Unit = command.Unit;
        Price = command.Price;
    }
    
    public void UpdateDetails(string supplyName, int quantity, string unit, decimal price)
    {
        SupplyName = supplyName;
        Quantity = quantity;
        Unit = unit;
        Price = price;
    }
}