using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Process.v1;

public sealed record ProcessWriteOffCommand(Guid Id) : IRequest<ProcessWriteOffResponse>;
