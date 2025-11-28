namespace WinesoftPlatform.API.Analytics.Domain.Model.Queries;

public record GetAnalyticsMetricsQuery(DateTime? StartDate, DateTime? EndDate);