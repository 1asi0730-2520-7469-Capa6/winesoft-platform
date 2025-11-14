using WinesoftPlatform.API.Dashboard.Domain.Model.Queries;
using WinesoftPlatform.API.Dashboard.Interfaces.REST.Resources;

namespace WinesoftPlatform.API.Dashboard.Domain.Services;

public interface IDashboardQueryService
{
    Task<IEnumerable<RecentOrderResource>> HandleGetRecentOrders();
    
    Task<IEnumerable<SupplyLevelResource>> HandleGetSupplyLevels();
   
    Task<IEnumerable<LowStockAlertResource>> HandleGetLowStockAlerts();
 
    Task<IEnumerable<SupplyRotationResource>> HandleGetSupplyRotation(GetDashboardMetricsQuery query);
    
    Task<CostsSummaryResource> HandleGetCostsSummary(GetDashboardMetricsQuery query); 
}