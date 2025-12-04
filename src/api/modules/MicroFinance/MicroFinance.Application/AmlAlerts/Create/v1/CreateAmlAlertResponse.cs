namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;

public sealed record CreateAmlAlertResponse(
    Guid Id,
    string AlertCode,
    string AlertType,
    string Severity,
    string Status);
