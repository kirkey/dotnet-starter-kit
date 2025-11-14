namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;

/// <summary>
/// Handler for terminating employee per Philippines Labor Code.
/// Computes separation pay based on years of service and termination reason.
/// </summary>
public sealed class TerminateEmployeeHandler(
    ILogger<TerminateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<TerminateEmployeeCommand, TerminateEmployeeResponse>
{
    public async Task<TerminateEmployeeResponse> Handle(
        TerminateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.Id);

        // Determine separation pay basis if not provided
        var separationPayBasis = request.SeparationPayBasis ?? DetermineSeparationPayBasis(request.TerminationReason);

        // Calculate separation pay if applicable
        decimal? separationPay = null;
        if (!string.IsNullOrWhiteSpace(separationPayBasis) && separationPayBasis != "None")
        {
            if (request.SeparationPayAmount.HasValue)
            {
                separationPay = request.SeparationPayAmount.Value;
            }
            else if (employee.BasicMonthlySalary.HasValue)
            {
                separationPay = CalculateSeparationPay(
                    employee.HireDate ?? DateTime.Now,
                    request.TerminationDate,
                    employee.BasicMonthlySalary.Value,
                    separationPayBasis);
            }
        }

        // Terminate employee
        employee.Terminate(
            request.TerminationDate,
            request.TerminationReason,
            request.TerminationMode,
            separationPayBasis,
            separationPay);

        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee {EmployeeId} terminated on {TerminationDate}. Reason: {Reason}, Separation Pay: {SeparationPay}",
            employee.Id,
            request.TerminationDate,
            request.TerminationReason,
            separationPay);

        return new TerminateEmployeeResponse(employee.Id, request.TerminationDate, separationPay);
    }

    /// <summary>
    /// Determines separation pay basis based on termination reason per Labor Code.
    /// </summary>
    private static string DetermineSeparationPayBasis(string terminationReason)
    {
        return terminationReason switch
        {
            // Authorized causes - entitled to separation pay
            "ReductionOfWorkforce" => "OneMonthPerYear",
            "Redundancy" => "OneMonthPerYear",
            "BusinessClosure" => "OneMonthPerYear",
            "Retirement" => "OneMonthPerYear",
            
            // Just causes - no separation pay
            "MisconductJustCause" => "None",
            "NeglectOfDuty" => "None",
            "BreachOfTrust" => "None",
            "CriminalOffense" => "None",
            "HabitualAbsenteeism" => "None",
            
            // Voluntary - no separation pay
            "ResignationVoluntary" => "None",
            "EndOfContract" => "None",
            "ProbationNotConfirmed" => "None",
            
            // Death - separation pay to heirs
            "Death" => "OneMonthPerYear",
            
            _ => "None"
        };
    }

    /// <summary>
    /// Calculates separation pay based on years of service.
    /// </summary>
    private static decimal CalculateSeparationPay(
        DateTime hireDate,
        DateTime terminationDate,
        decimal basicMonthlySalary,
        string separationPayBasis)
    {
        var yearsOfService = (terminationDate - hireDate).TotalDays / 365.25;

        return separationPayBasis switch
        {
            "HalfMonthPerYear" => basicMonthlySalary * 0.5m * (decimal)yearsOfService,
            "OneMonthPerYear" => basicMonthlySalary * (decimal)yearsOfService,
            _ => 0m
        };
    }
}

