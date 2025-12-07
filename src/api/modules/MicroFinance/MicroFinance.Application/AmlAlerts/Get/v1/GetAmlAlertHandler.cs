using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;

public sealed class GetAmlAlertHandler(
    [FromKeyedServices("microfinance:amlalerts")] IReadRepository<AmlAlert> repository,
    ILogger<GetAmlAlertHandler> logger)
    : IRequestHandler<GetAmlAlertRequest, AmlAlertResponse>
{
    public async Task<AmlAlertResponse> Handle(GetAmlAlertRequest request, CancellationToken cancellationToken)
    {
        var alert = await repository.FirstOrDefaultAsync(new AmlAlertByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"AML alert {request.Id} not found");

        logger.LogInformation("Retrieved AML alert {Id}", alert.Id);

        return new AmlAlertResponse(
            alert.Id,
            alert.AlertCode,
            alert.MemberId,
            alert.TransactionId,
            alert.AlertType,
            alert.Severity,
            alert.Status,
            alert.TriggerRule,
            alert.Description,
            alert.TransactionAmount,
            alert.AlertedAt,
            alert.InvestigationStartedAt,
            alert.ResolvedAt,
            alert.AssignedToId,
            alert.ResolutionNotes,
            alert.SarReference,
            alert.SarFiledDate,
            alert.RequiresReporting);
    }
}
