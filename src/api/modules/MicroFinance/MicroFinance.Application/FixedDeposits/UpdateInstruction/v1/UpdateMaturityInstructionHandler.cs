using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

/// <summary>
/// Handler for updating maturity instruction.
/// </summary>
public sealed class UpdateMaturityInstructionHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository,
    ILogger<UpdateMaturityInstructionHandler> logger)
    : IRequestHandler<UpdateMaturityInstructionCommand, UpdateMaturityInstructionResponse>
{
    public async Task<UpdateMaturityInstructionResponse> Handle(UpdateMaturityInstructionCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new NotFoundException($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.UpdateMaturityInstruction(request.Instruction);

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Updated maturity instruction for fixed deposit {DepositId}", request.DepositId);

        return new UpdateMaturityInstructionResponse(
            deposit.Id,
            deposit.MaturityInstruction,
            "Maturity instruction updated successfully.");
    }
}
