namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;

public sealed record AssignAmlAlertResponse(Guid Id, string Status, Guid? AssignedToId);
