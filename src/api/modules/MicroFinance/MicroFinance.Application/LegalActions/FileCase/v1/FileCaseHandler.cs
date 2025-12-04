using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.FileCase.v1;

public sealed class FileCaseHandler(
    [FromKeyedServices("microfinance:legalactions")] IRepository<LegalAction> repository,
    ILogger<FileCaseHandler> logger)
    : IRequestHandler<FileCaseCommand, FileCaseResponse>
{
    public async Task<FileCaseResponse> Handle(
        FileCaseCommand request,
        CancellationToken cancellationToken)
    {
        var action = await repository.FirstOrDefaultAsync(
            new LegalActionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Legal action {request.Id} not found");

        action.FileCase(request.FiledDate, request.CaseReference, request.CourtName, request.CourtFees);
        await repository.UpdateAsync(action, cancellationToken);

        logger.LogInformation("Legal case filed: {LegalActionId}", action.Id);

        return new FileCaseResponse(action.Id, action.Status, request.CaseReference);
    }
}
