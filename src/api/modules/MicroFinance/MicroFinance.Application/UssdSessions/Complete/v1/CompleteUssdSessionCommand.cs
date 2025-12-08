using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Complete.v1;

/// <summary>
/// Command to complete a USSD session.
/// </summary>
/// <param name="Id">The unique identifier of the USSD session to complete.</param>
/// <param name="FinalOutput">Optional final output message to display.</param>
public sealed record CompleteUssdSessionCommand(
    DefaultIdType Id,
    string? FinalOutput = null) : IRequest<CompleteUssdSessionResponse>;
