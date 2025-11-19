namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Handler for creating employee bank account with validation.
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

        // Validate employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Create bank account
        var account = BankAccount.Create(
            request.EmployeeId,
            request.AccountNumber,
            request.RoutingNumber,
            request.BankName,
            request.AccountType,
            request.AccountHolderName,
            request.SwiftCode,
            request.Iban);

        // Add notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
            account.Update(notes: request.Notes);

        await repository.AddAsync(account, cancellationToken);

        logger.LogInformation(
            "Bank account created: ID {Id}, Employee {EmployeeId}, Bank {Bank}, Last4 {Last4}, Type {Type}",
            account.Id,
            account.EmployeeId,
            account.BankName,
            account.Last4Digits,
            account.AccountType);

        return new CreateBankAccountResponse(
            account.Id,
            account.EmployeeId,
            account.BankName,
            account.Last4Digits,
            account.AccountType,
            account.IsPrimary);
    }
}

