using Microsoft.AspNetCore.Mvc;
using WinesoftPlatform.API.Dashboard.Domain.Model.Queries;
using WinesoftPlatform.API.Dashboard.Domain.Services;

namespace WinesoftPlatform.API.Dashboard.Interfaces.REST.Controllers;

/**
 * API Controller for Dashboard Bounded Context.
 * Define the endpoints that the frontend will consume.
 */
[ApiController]
[Route("api/v1/dashboard")] // Base path for this controller
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    // Injects the Service Interface
    private readonly IDashboardQueryService _dashboardQueryService;

    public DashboardController(IDashboardQueryService dashboardQueryService)
    {
        _dashboardQueryService = dashboardQueryService;
    }

    // Define an endpoint for each widget

    /**
     * Endpoint for the "Recent Orders (Filtered)" widget
     */
    [HttpGet("recent-orders")]
    public async Task<IActionResult> GetRecentOrders()
    {
        var orders = await _dashboardQueryService.HandleGetRecentOrders();
        return Ok(orders);
    }

    /**
     * Endpoint for the "Supply Levels (Current)" widget
     */
    [HttpGet("supply-levels")]
    public async Task<IActionResult> GetSupplyLevels()
    {
        var levels = await _dashboardQueryService.HandleGetSupplyLevels();
        return Ok(levels);
    }

    /**
     * Endpoint for the "Low Stock Alerts" widget
     */
    [HttpGet("low-stock-alerts")]
    public async Task<IActionResult> GetLowStockAlerts()
    {
        var alerts = await _dashboardQueryService.HandleGetLowStockAlerts();
        return Ok(alerts);
    }

    /**
     * Endpoint for the "Daily Supply Rotation" chart (with date filter)
     */
    [HttpGet("supply-rotation")]
    public async Task<IActionResult> GetSupplyRotation([FromQuery] GetDashboardMetricsQuery query)
    {
        var data = await _dashboardQueryService.HandleGetSupplyRotation(query);
        return Ok(data);
    }
    
    /**
     * Endpoint for the "Costs Summary" widget (with date filter)
     */
    [HttpGet("costs-summary")]
    public async Task<IActionResult> GetCostsSummary([FromQuery] GetDashboardMetricsQuery query)
    {
        var data = await _dashboardQueryService.HandleGetCostsSummary(query);
        return Ok(data);
    }
}