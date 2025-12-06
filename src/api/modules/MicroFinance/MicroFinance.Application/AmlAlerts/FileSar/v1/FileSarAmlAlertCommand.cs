using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.FileSar.v1;

public sealed record FileSarAmlAlertCommand(
    Guid Id,
    string SarReference,
    DateOnly FiledDate) : IRequest<FileSarAmlAlertResponse>;
