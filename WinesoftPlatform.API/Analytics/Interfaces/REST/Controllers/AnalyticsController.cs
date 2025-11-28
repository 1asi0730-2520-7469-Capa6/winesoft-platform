using Microsoft.AspNetCore.Mvc;
using WinesoftPlatform.API.Analytics.Domain.Model.Queries;
using WinesoftPlatform.API.Analytics.Domain.Services;

namespace WinesoftPlatform.API.Analytics.Interfaces.REST.Controllers;

/**
 * API Controller for Analytics Bounded Context.
 * Define the endpoints that the frontend will consume.
 */
[ApiController]
[Route("api/v1/analytics")] // Base path for this controller
[Produces("application/json")]
public class AnalyticsController : ControllerBase
{
    // Injects the Service Interface
    private readonly IAnalyticsQueryService _analyticsQueryService;

    public AnalyticsController(IAnalyticsQueryService analyticsQueryService)
    {
        _analyticsQueryService = analyticsQueryService;
    }

    // Define an endpoint for each widget

    /**
     * Endpoint for the "Recent Orders (Filtered)" widget
     */
    [HttpGet("recent-orders")]
    public async Task<IActionResult> GetRecentOrders()
    {
        var orders = await _analyticsQueryService.HandleGetRecentOrders();
        return Ok(orders);
    }

    /**
     * Endpoint for the "Supply Levels (Current)" widget
     */
    [HttpGet("supply-levels")]
    public async Task<IActionResult> GetSupplyLevels()
    {
        var levels = await _analyticsQueryService.HandleGetSupplyLevels();
        return Ok(levels);
    }

    /**
     * Endpoint for the "Low Stock Alerts" widget
     */
    [HttpGet("low-stock-alerts")]
    public async Task<IActionResult> GetLowStockAlerts()
    {
        var alerts = await _analyticsQueryService.HandleGetLowStockAlerts();
        return Ok(alerts);
    }

    /**
     * Endpoint for the "Daily Supply Rotation" chart (with date filter)
     */
    [HttpGet("supply-rotation")]
    public async Task<IActionResult> GetSupplyRotation([FromQuery] GetAnalyticsMetricsQuery query)
    {
        var data = await _analyticsQueryService.HandleGetSupplyRotation(query);
        return Ok(data);
    }
    
    /**
     * Endpoint for the "Costs Summary" widget (with date filter)
     */
    [HttpGet("costs-summary")]
    public async Task<IActionResult> GetCostsSummary([FromQuery] GetAnalyticsMetricsQuery query)
    {
        var data = await _analyticsQueryService.HandleGetCostsSummary(query);
        return Ok(data);
    }
}