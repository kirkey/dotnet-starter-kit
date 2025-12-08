namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;

public sealed record AssignAmlAlertResponse(DefaultIdType Id, string Status, DefaultIdType? AssignedToId);
