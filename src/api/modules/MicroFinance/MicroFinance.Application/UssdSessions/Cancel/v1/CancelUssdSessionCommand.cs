using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Cancel.v1;

/// <summary>
/// Command to cancel a USSD session.
/// </summary>
/// <param name="Id">The unique identifier of the USSD session to cancel.</param>
public sealed record CancelUssdSessionCommand(DefaultIdType Id) : IRequest<CancelUssdSessionResponse>;
