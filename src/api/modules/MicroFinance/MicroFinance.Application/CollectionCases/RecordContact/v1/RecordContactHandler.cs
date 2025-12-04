using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordContact.v1;

public sealed class RecordContactHandler(
    ILogger<RecordContactHandler> logger,
    [FromKeyedServices("microfinance:collectioncases")] IRepository<CollectionCase> repository)
    : IRequestHandler<RecordContactCommand, RecordContactResponse>
{
    public async Task<RecordContactResponse> Handle(RecordContactCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collectionCase = await repository.FirstOrDefaultAsync(
            new CollectionCaseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collectionCase is null)
            throw new NotFoundException($"Collection case with ID {request.Id} not found.");

        collectionCase.RecordContact(request.ContactDate, request.NextFollowUp);

        await repository.UpdateAsync(collectionCase, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Contact recorded for case {CaseId}, Attempts: {Attempts}",
            collectionCase.Id, collectionCase.ContactAttempts);

        return new RecordContactResponse(
            collectionCase.Id,
            collectionCase.LastContactDate!.Value,
            collectionCase.NextFollowUpDate,
            collectionCase.ContactAttempts,
            collectionCase.Status);
    }
}
