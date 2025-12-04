using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Close.v1;

/// <summary>
/// Handler for closing a customer case.
/// </summary>
public sealed class CloseCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IRepository<CustomerCase> repository,
    ILogger<CloseCustomerCaseHandler> logger)
    : IRequestHandler<CloseCustomerCaseCommand, CloseCustomerCaseResponse>
{
    public async Task<CloseCustomerCaseResponse> Handle(CloseCustomerCaseCommand request, CancellationToken cancellationToken)
    {
        var customerCase = await repository.GetByIdAsync(request.CaseId, cancellationToken);

        if (customerCase is null)
        {
            throw new NotFoundException($"Customer case with ID {request.CaseId} not found.");
        }

        customerCase.Close(request.SatisfactionScore, request.Feedback);
        await repository.UpdateAsync(customerCase, cancellationToken);

        logger.LogInformation("Customer case {CaseId} closed with satisfaction score {Score}",
            request.CaseId, request.SatisfactionScore);

        return new CloseCustomerCaseResponse(
            customerCase.Id,
            customerCase.Status,
            customerCase.ClosedAt!.Value);
    }
}
