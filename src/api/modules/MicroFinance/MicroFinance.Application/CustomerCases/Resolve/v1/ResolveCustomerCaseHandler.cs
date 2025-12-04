using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Resolve.v1;

/// <summary>
/// Handler for resolving a customer case.
/// </summary>
public sealed class ResolveCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IRepository<CustomerCase> repository,
    ILogger<ResolveCustomerCaseHandler> logger)
    : IRequestHandler<ResolveCustomerCaseCommand, ResolveCustomerCaseResponse>
{
    public async Task<ResolveCustomerCaseResponse> Handle(ResolveCustomerCaseCommand request, CancellationToken cancellationToken)
    {
        var customerCase = await repository.GetByIdAsync(request.CaseId, cancellationToken);

        if (customerCase is null)
        {
            throw new NotFoundException($"Customer case with ID {request.CaseId} not found.");
        }

        customerCase.Resolve(request.Resolution);
        await repository.UpdateAsync(customerCase, cancellationToken);

        logger.LogInformation("Customer case {CaseId} resolved", request.CaseId);

        return new ResolveCustomerCaseResponse(
            customerCase.Id,
            customerCase.Status,
            customerCase.ResolvedAt!.Value);
    }
}
