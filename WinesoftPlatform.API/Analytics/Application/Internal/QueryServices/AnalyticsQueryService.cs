using Microsoft.EntityFrameworkCore;
using WinesoftPlatform.API.Analytics.Domain.Model.Queries;
using WinesoftPlatform.API.Analytics.Domain.Services;
using WinesoftPlatform.API.Analytics.Interfaces.REST.Resources;
using WinesoftPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace WinesoftPlatform.API.Analytics.Application.Internal.QueryServices;

/// <summary>
/// Query service for analytics operations.
/// </summary>
/// <param name="context">
/// The <see cref="AppDbContext"/> database context.
/// </param>
public class AnalyticsQueryService(AppDbContext context) : IAnalyticsQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<PurchaseOrderResource>> Handle(GetPurchaseOrdersLast7DaysQuery query)
    {
        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
        
        return await context.Orders
            .AsNoTracking()
            .Where(o => o.CreatedDate >= sevenDaysAgo)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new PurchaseOrderResource(
                o.Id,
                o.Status,
                o.CreatedDate.Value.DateTime,
                o.ProductId,
                o.Quantity
            ))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupplyLevelResource>> HandleGetSupplyLevels()
    {
        return await context.Supplies
            .AsNoTracking()
            .GroupBy(s => s.SupplyName)
            .Select(g => new SupplyLevelResource(
                g.Key,
                g.Sum(s => s.Quantity)
            ))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LowStockAlertResource>> HandleGetLowStockAlerts()
    {
        const int defaultThreshold = 30;

        return await context.Supplies
            .AsNoTracking()
            .Where(s => s.Quantity < defaultThreshold)
            .Select(s => new LowStockAlertResource(
                s.SupplyName, 
                s.Quantity, 
                defaultThreshold
            ))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SupplyRotationResource>> HandleGetSupplyRotation(GetAnalyticsMetricsQuery query)
    {
        var endDate = query.EndDate ?? DateTime.UtcNow;
        var startDate = query.StartDate ?? endDate.AddDays(-7);

        var rawSupplies = await context.Supplies
            .AsNoTracking()
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .ToListAsync();
        
        var result = rawSupplies
            .GroupBy(s => s.Date.Date)
            .Select(g => new SupplyRotationResource(
                g.Key,
                g.Count()
            ))
            .OrderBy(r => r.Day)
            .ToList();

        return result;
    }

    /// <inheritdoc />
    public async Task<CostsSummaryResource> HandleGetCostsSummary(GetAnalyticsMetricsQuery query)
    {
        var endDate = query.EndDate ?? DateTime.UtcNow;
        var startDate = query.StartDate ?? endDate.AddDays(-30);

        var totalCost = await context.Orders
            .AsNoTracking()
            .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate)
            .Join(context.Supplies,
                order => order.ProductId,
                supply => supply.Id,
                (order, supply) => new { order.Quantity, supply.Price }
            )
            .SumAsync(x => (double)x.Quantity * (double)x.Price);

        return new CostsSummaryResource(totalCost, startDate, endDate);
    }
}