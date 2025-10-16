using Accounting.Application.Banks.Queries;
using Accounting.Application.ChartOfAccounts.Specs;
using Accounting.Application.Checks.Exceptions;
using Accounting.Application.Checks.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Handler for creating a bundle of new checks.
/// Creates all checks in the specified range (e.g., 3453000-3453500) as a single transaction.
/// </summary>
public sealed class CheckCreateHandler(
    ILogger<CheckCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> checkRepository,
    [FromKeyedServices("accounting")] IRepository<Bank> bankRepository,
    [FromKeyedServices("accounting")] IRepository<ChartOfAccount> chartOfAccountRepository)
    : IRequestHandler<CheckCreateCommand, CheckCreateResponse>
{
    public async Task<CheckCreateResponse> Handle(CheckCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Parse check number range
        if (!long.TryParse(request.StartCheckNumber, out var startNum) ||
            !long.TryParse(request.EndCheckNumber, out var endNum))
        {
            throw new ArgumentException("Check numbers must be numeric.");
        }

        // Calculate range size
        int rangeSize = (int)(endNum - startNum + 1);
        
        logger.LogInformation("Creating check bundle: {StartCheckNumber}-{EndCheckNumber} ({RangeSize} checks)",
            request.StartCheckNumber, request.EndCheckNumber, rangeSize);

        // Fetch bank account name once for all checks
        string? bankAccountName = null;
        if (!string.IsNullOrWhiteSpace(request.BankAccountCode))
        {
            var chartOfAccount = await chartOfAccountRepository.FirstOrDefaultAsync(
                new ChartOfAccountByCodeSpec(request.BankAccountCode),
                cancellationToken);

            if (chartOfAccount != null)
            {
                bankAccountName = chartOfAccount.AccountName;
            }
        }

        // Fetch bank name once for all checks
        string? bankName = null;
        if (request.BankId.HasValue && request.BankId.Value != DefaultIdType.Empty)
        {
            var bank = await bankRepository.FirstOrDefaultAsync(
                new BankByIdSpec(request.BankId.Value),
                cancellationToken);

            if (bank != null)
            {
                bankName = bank.Name;
            }
        }

        // Create all checks in the range
        var checksToAdd = new List<Check>();
        var failedNumbers = new List<string>();

        for (long checkNum = startNum; checkNum <= endNum; checkNum++)
        {
            string checkNumber = checkNum.ToString("D" + request.StartCheckNumber.Length);

            // Check for duplicate check number within the same bank account
            var existingCheck = await checkRepository.FirstOrDefaultAsync(
                new CheckByNumberAndAccountSpec(checkNumber, request.BankAccountCode),
                cancellationToken);

            if (existingCheck != null)
            {
                failedNumbers.Add(checkNumber);
                logger.LogWarning("Check number already exists: {CheckNumber} for account {BankAccountCode}",
                    checkNumber, request.BankAccountCode);
                continue;
            }

            // Create check entity
            var check = Check.Create(
                checkNumber,
                request.BankAccountCode,
                bankAccountName,
                request.BankId,
                bankName,
                request.Description,
                request.Notes);

            checksToAdd.Add(check);
        }

        // If no valid checks to add, throw exception
        if (checksToAdd.Count == 0)
        {
            throw new CheckNumberAlreadyExistsException(
                $"Range {request.StartCheckNumber}-{request.EndCheckNumber}", request.BankAccountCode);
        }

        // Add all valid checks to repository
        await checkRepository.AddRangeAsync(checksToAdd, cancellationToken);
        await checkRepository.SaveChangesAsync(cancellationToken);

        int checksCreated = checksToAdd.Count;
        logger.LogInformation("Check bundle created: {ChecksCreated}/{TotalRange} checks registered. " +
            "Start: {StartCheckNumber}, End: {EndCheckNumber}, Failed: {FailedCount}",
            checksCreated, rangeSize, request.StartCheckNumber, request.EndCheckNumber, failedNumbers.Count);

        // Return response with bundle info (using first check ID as reference)
        return new CheckCreateResponse(
            checksToAdd[0].Id,
            request.StartCheckNumber,
            request.EndCheckNumber,
            checksCreated);
    }
}
