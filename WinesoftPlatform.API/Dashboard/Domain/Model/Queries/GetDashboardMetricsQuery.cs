namespace WinesoftPlatform.API.Dashboard.Domain.Model.Queries;

public record GetDashboardMetricsQuery(DateTime? StartDate, DateTime? EndDate);