using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Update.v1;

/// <summary>
/// Command to update an existing collection action.
/// </summary>
public sealed record UpdateCollectionActionCommand(
    DefaultIdType Id,
    string? Outcome = null,
    string? Description = null,
    string? ContactMethod = null,
    string? PhoneNumberCalled = null,
    string? ContactPerson = null,
    decimal? PromisedAmount = null,
    DateOnly? PromisedDate = null,
    DateOnly? NextFollowUpDate = null,
    int? DurationMinutes = null,
    decimal? Latitude = null,
    decimal? Longitude = null,
    string? Notes = null)
    : IRequest<UpdateCollectionActionResponse>;
