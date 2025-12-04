namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;

public sealed record MfiConfigurationResponse(
    Guid Id,
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
    Guid? BranchId,
    int DisplayOrder,
    DateTimeOffset CreatedOn);
