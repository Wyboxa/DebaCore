namespace Debales.Application.Common;

public sealed record AgingItemDto(
    Guid Id,
    string Number,
    Guid ThirdPartyId,
    string ThirdPartyName,
    DateOnly DueDate,
    decimal OriginalAmount,
    decimal PendingAmount,
    int DaysOverdue);

public sealed record AgingBucketDto(string Label, int Count, decimal Amount);

public sealed record AgingReportDto(
    DateOnly AsOf,
    decimal TotalPending,
    decimal TotalOverdue,
    AgingBucketDto Current,
    AgingBucketDto Bucket1_30,
    AgingBucketDto Bucket31_60,
    AgingBucketDto Bucket61_90,
    AgingBucketDto BucketOver90,
    IReadOnlyList<AgingItemDto> Items);
