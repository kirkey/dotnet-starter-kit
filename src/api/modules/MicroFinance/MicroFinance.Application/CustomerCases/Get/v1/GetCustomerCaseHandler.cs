using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Get.v1;

/// <summary>
/// Handler for getting a customer case by ID.
/// </summary>
public sealed class GetCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IReadRepository<CustomerCase> repository,
    ILogger<GetCustomerCaseHandler> logger)
    : IRequestHandler<GetCustomerCaseRequest, CustomerCaseResponse>
{
    public async Task<CustomerCaseResponse> Handle(GetCustomerCaseRequest request, CancellationToken cancellationToken)
    {
        var spec = new CustomerCaseByIdSpec(request.Id);
        var customerCase = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (customerCase is null)
        {
            throw new NotFoundException($"Customer case with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved customer case {CustomerCaseId}", request.Id);

        return customerCase;
    }
}
