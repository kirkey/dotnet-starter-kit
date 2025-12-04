using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Create.v1;

/// <summary>
/// Handler for creating a new customer case.
/// </summary>
public sealed class CreateCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IRepository<CustomerCase> repository,
    ILogger<CreateCustomerCaseHandler> logger)
    : IRequestHandler<CreateCustomerCaseCommand, CreateCustomerCaseResponse>
{
    public async Task<CreateCustomerCaseResponse> Handle(CreateCustomerCaseCommand request, CancellationToken cancellationToken)
    {
        var customerCase = CustomerCase.Create(
            request.CaseNumber,
            request.MemberId,
            request.Subject,
            request.Category,
            request.Description,
            request.Channel,
            request.Priority,
            request.SlaHours);

        await repository.AddAsync(customerCase, cancellationToken);

        logger.LogInformation("Customer case {CaseNumber} created for member {MemberId}",
            request.CaseNumber, request.MemberId);

        return new CreateCustomerCaseResponse(
            customerCase.Id,
            customerCase.CaseNumber,
            customerCase.Subject,
            customerCase.Status);
    }
}
