namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Handler for creating a bank account.
/// </summary>
public sealed class CreateBankAccountHandler(
    ILogger<CreateBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateBankAccountCommand, CreateBankAccountResponse>
{
    public async Task<CreateBankAccountResponse> Handle(
        CreateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new Exception($"Employee not found: {request.EmployeeId}");

        var bankAccount = BankAccount.Create(
            request.EmployeeId,
            request.AccountNumber,
            request.RoutingNumber,
            request.BankName,
            request.AccountType,
            request.AccountHolderName,
            request.SwiftCode,
            request.Iban,
            request.CurrencyCode);

        await repository.AddAsync(bankAccount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bank account created with ID {BankAccountId}, Employee {EmployeeId}, Bank {BankName}",
            bankAccount.Id,
            request.EmployeeId,
            request.BankName);

        return new CreateBankAccountResponse(bankAccount.Id);
    }
}

