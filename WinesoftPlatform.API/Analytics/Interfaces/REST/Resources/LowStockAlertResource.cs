namespace WinesoftPlatform.API.Analytics.Interfaces.REST.Resources;

public record LowStockAlertResource(string ProductName, int CurrentStock, int Threshold);