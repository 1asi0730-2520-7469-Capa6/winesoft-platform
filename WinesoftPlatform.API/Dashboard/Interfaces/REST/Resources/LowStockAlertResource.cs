namespace WinesoftPlatform.API.Dashboard.Interfaces.REST.Resources;

public record LowStockAlertResource(string ProductName, int CurrentStock, int Threshold);