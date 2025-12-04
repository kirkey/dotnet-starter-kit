using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Assign.v1;

/// <summary>
/// Handler for assigning a customer case.
/// </summary>
public sealed class AssignCustomerCaseHandler(
    [FromKeyedServices("microfinance:customercases")] IRepository<CustomerCase> repository,
    ILogger<AssignCustomerCaseHandler> logger)
    : IRequestHandler<AssignCustomerCaseCommand, AssignCustomerCaseResponse>
{
    public async Task<AssignCustomerCaseResponse> Handle(AssignCustomerCaseCommand request, CancellationToken cancellationToken)
    {
        var customerCase = await repository.GetByIdAsync(request.CaseId, cancellationToken);

        if (customerCase is null)
        {
            throw new NotFoundException($"Customer case with ID {request.CaseId} not found.");
        }

        customerCase.Assign(request.StaffId);
        await repository.UpdateAsync(customerCase, cancellationToken);

        logger.LogInformation("Customer case {CaseId} assigned to staff {StaffId}",
            request.CaseId, request.StaffId);

        return new AssignCustomerCaseResponse(
            customerCase.Id,
            customerCase.AssignedToId!.Value,
            customerCase.Status);
    }
}
