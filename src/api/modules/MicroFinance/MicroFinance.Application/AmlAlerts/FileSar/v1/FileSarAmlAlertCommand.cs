using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.FileSar.v1;

public sealed record FileSarAmlAlertCommand(
    DefaultIdType Id,
    string SarReference,
    DateOnly FiledDate) : IRequest<FileSarAmlAlertResponse>;
