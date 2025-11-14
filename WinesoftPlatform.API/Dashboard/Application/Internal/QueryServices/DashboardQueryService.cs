using Microsoft.EntityFrameworkCore;
using WinesoftPlatform.API.Dashboard.Domain.Model.Queries;
using WinesoftPlatform.API.Dashboard.Domain.Services;
using WinesoftPlatform.API.Dashboard.Interfaces.REST.Resources;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace WinesoftPlatform.API.Dashboard.Application.Internal.QueryServices;

/**
 * Implementation of the Dashboard query service.
 */
public class DashboardQueryService : IDashboardQueryService
{
    private readonly AppDbContext _context;

    public DashboardQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecentOrderResource>> HandleGetRecentOrders()
    {
        // Read from _context.Orders
        return await _context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedDate)
            .Take(5)
            .Select(o => new RecentOrderResource(
                o.Id,
                o.Status,
                o.CreatedDate.Value.DateTime, // Convert DateTimeOffset? to DateTime
                o.ProductId,
                o.Quantity
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplyLevelResource>> HandleGetSupplyLevels()
    {
        return await _context.Supplies
            .AsNoTracking()
            .GroupBy(s => s.SupplyName)
            .Select(g => new SupplyLevelResource(
                g.Key,
                g.Sum(s => s.Quantity) // Add up all the quantities of that product
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<LowStockAlertResource>> HandleGetLowStockAlerts()
    {
        // return await _context.Supplies
        //     .AsNoTracking()
        //     .Where(s => s.Quantity <= s.MinStock) 
        //     .Select(s => new LowStockAlertResource(s.SupplyName, s.Quantity, s.MinStock))
        //     .ToListAsync();

        return await Task.FromResult(new List<LowStockAlertResource>().AsEnumerable());
    }

    public async Task<IEnumerable<SupplyRotationResource>> HandleGetSupplyRotation(GetDashboardMetricsQuery query)
    {
        var endDate = query.EndDate ?? DateTime.UtcNow;
        var startDate = query.StartDate ?? endDate.AddDays(-7);

        return await _context.Supplies
            .AsNoTracking()
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .GroupBy(s => s.Date.Date) // Group by day
            .Select(g => new SupplyRotationResource(
                g.Key,
                g.Count()   // The number of movements entries
            ))
            .OrderBy(r => r.Day)
            .ToListAsync();
    }

    public async Task<CostsSummaryResource> HandleGetCostsSummary(GetDashboardMetricsQuery query)
    {
        var endDate = query.EndDate ?? DateTime.UtcNow;
        var startDate = query.StartDate ?? endDate.AddDays(-30);

        // This query combines Orders and Supplies to calculate (Quantity * Price)
        var totalCost = await _context.Orders
            .AsNoTracking()
            .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate)
            .Join(_context.Supplies, //The JOIN with the Supplies table
                order => order.ProductId,
                supply => supply.Id,
                (order, supply) => new { order.Quantity, supply.Price }
            )
            .SumAsync(x => (double)x.Quantity * (double)x.Price); //Total (Quantity * Price)

        return new CostsSummaryResource(totalCost, startDate, endDate);
    }
}