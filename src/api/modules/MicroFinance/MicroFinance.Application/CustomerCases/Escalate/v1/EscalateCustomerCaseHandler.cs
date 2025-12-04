using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Escalate.v1;

/// <summary>
/// Handler for escalating a customer case.
/// </summary>
public sealed class EscalateCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IRepository<CustomerCase> repository,
    ILogger<EscalateCustomerCaseHandler> logger)
    : IRequestHandler<EscalateCustomerCaseCommand, EscalateCustomerCaseResponse>
{
    public async Task<EscalateCustomerCaseResponse> Handle(EscalateCustomerCaseCommand request, CancellationToken cancellationToken)
    {
        var customerCase = await repository.GetByIdAsync(request.CaseId, cancellationToken);

        if (customerCase is null)
        {
            throw new NotFoundException($"Customer case with ID {request.CaseId} not found.");
        }

        customerCase.Escalate(request.EscalatedToId, request.Reason);
        await repository.UpdateAsync(customerCase, cancellationToken);

        logger.LogInformation("Customer case {CaseId} escalated to level {Level}",
            request.CaseId, customerCase.EscalationLevel);

        return new EscalateCustomerCaseResponse(
            customerCase.Id,
            customerCase.EscalationLevel,
            customerCase.Status);
    }
}
