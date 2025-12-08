namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;

public sealed record MfiConfigurationResponse(
    DefaultIdType Id,
    string Key,
    string Value,
    string Category,
    string DataType,
    string? Description,
    bool IsEncrypted,
    bool IsEditable,
    bool RequiresRestart,
    string? DefaultValue,
    string? ValidationRules,
    DefaultIdType? BranchId,
    int DisplayOrder,
    DateTimeOffset CreatedOn);
