using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Timeout.v1;

/// <summary>
/// Command to mark a USSD session as timed out.
/// </summary>
/// <param name="Id">The unique identifier of the USSD session to timeout.</param>
public sealed record TimeoutUssdSessionCommand(DefaultIdType Id) : IRequest<TimeoutUssdSessionResponse>;
