using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.End.v1;

/// <summary>
/// Handler for ending a designation assignment.
/// </summary>
public sealed class EndDesignationAssignmentHandler(
    ILogger<EndDesignationAssignmentHandler> logger,
    [FromKeyedServices("hr:designationassignments")] IRepository<DesignationAssignment> repository)
    : IRequestHandler<EndDesignationAssignmentCommand, EndDesignationAssignmentResponse>
{
    public async Task<EndDesignationAssignmentResponse> Handle(
        EndDesignationAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignment = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
            throw new DesignationAssignmentNotFoundException(request.Id);

        if (assignment.EndDate.HasValue)
            throw new InvalidOperationException($"This assignment has already ended on {assignment.EndDate:MMM d, yyyy}.");

        assignment.SetEndDate(request.EndDate);

        await repository.UpdateAsync(assignment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Designation assignment {AssignmentId} ended on {EndDate}",
            assignment.Id,
            request.EndDate);

        return new EndDesignationAssignmentResponse(assignment.Id);
    }
}

