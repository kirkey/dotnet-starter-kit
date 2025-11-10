namespace Accounting.Application.TrialBalance.Get.v1;

/// <summary>
/// Handler for retrieving a trial balance by ID.
/// </summary>
public sealed class TrialBalanceGetHandler(
    [FromKeyedServices("accounting:trial-balance")] IReadRepository<Domain.Entities.TrialBalance> repository,
    ILogger<TrialBalanceGetHandler> logger)
    : IRequestHandler<TrialBalanceGetRequest, TrialBalanceGetResponse>
{
    public async Task<TrialBalanceGetResponse> Handle(TrialBalanceGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Retrieving trial balance with ID {TrialBalanceId}", request.Id);

        var trialBalance = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (trialBalance == null)
        {
            logger.LogWarning("Trial balance with ID {TrialBalanceId} not found", request.Id);
            throw new NotFoundException($"Trial balance with ID {request.Id} not found");
        }

        logger.LogInformation("Trial balance {TrialBalanceNumber} retrieved successfully", trialBalance.TrialBalanceNumber);

        return new TrialBalanceGetResponse
        {
            Id = trialBalance.Id,
            TrialBalanceNumber = trialBalance.TrialBalanceNumber,
            PeriodId = trialBalance.PeriodId,
            GeneratedDate = trialBalance.GeneratedDate,
            PeriodStartDate = trialBalance.PeriodStartDate,
            PeriodEndDate = trialBalance.PeriodEndDate,
            TotalDebits = trialBalance.TotalDebits,
            TotalCredits = trialBalance.TotalCredits,
            TotalAssets = trialBalance.TotalAssets,
            TotalLiabilities = trialBalance.TotalLiabilities,
            TotalEquity = trialBalance.TotalEquity,
            TotalRevenue = trialBalance.TotalRevenue,
            TotalExpenses = trialBalance.TotalExpenses,
            NetIncome = trialBalance.NetIncome,
            IsBalanced = trialBalance.IsBalanced,
            OutOfBalanceAmount = trialBalance.OutOfBalanceAmount,
            AccountingEquationBalances = trialBalance.AccountingEquationBalances,
            Status = trialBalance.Status,
            IncludeZeroBalances = trialBalance.IncludeZeroBalances,
            AccountCount = trialBalance.AccountCount,
            FinalizedDate = trialBalance.FinalizedDate,
            FinalizedBy = trialBalance.FinalizedBy,
            Description = trialBalance.Description,
            Notes = trialBalance.Notes,
            LineItems = trialBalance.LineItems.Select(li => new TrialBalanceLineItemDto
            {
                AccountCode = li.AccountCode,
                AccountName = li.AccountName,
                AccountType = li.AccountType,
                DebitBalance = li.DebitBalance,
                CreditBalance = li.CreditBalance,
                NetBalance = li.NetBalance
            }).ToList(),
            CreatedOn = trialBalance.CreatedOn.DateTime
        };
    }
}
