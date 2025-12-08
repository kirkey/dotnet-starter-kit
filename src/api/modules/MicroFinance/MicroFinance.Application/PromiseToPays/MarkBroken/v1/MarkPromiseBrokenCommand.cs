using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.MarkBroken.v1;

/// <summary>
/// Command to mark a promise to pay as broken.
/// </summary>
public sealed record MarkPromiseBrokenCommand(DefaultIdType PromiseId, string Reason) : IRequest<MarkPromiseBrokenResponse>;
