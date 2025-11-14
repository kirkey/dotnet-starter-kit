using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when payroll is created.
/// </summary>
public record PayrollCreated : DomainEvent
{
    public Payroll Payroll { get; init; } = default!;
}

/// <summary>
/// Event raised when payroll is processed.
/// </summary>
public record PayrollProcessed : DomainEvent
{
    public Payroll Payroll { get; init; } = default!;
}

/// <summary>
/// Event raised when payroll processing is completed.
/// </summary>
public record PayrollCompleted : DomainEvent
{
    public Payroll Payroll { get; init; } = default!;
}

/// <summary>
/// Event raised when payroll is posted to GL.
/// </summary>
public record PayrollPosted : DomainEvent
{
    public Payroll Payroll { get; init; } = default!;
}

/// <summary>
/// Event raised when payroll is marked as paid.
/// </summary>
public record PayrollPaid : DomainEvent
{
    public Payroll Payroll { get; init; } = default!;
}

/// <summary>
/// Event raised when benefit enrollment is created.
/// </summary>
public record BenefitEnrollmentCreated : DomainEvent
{
    public BenefitEnrollment Enrollment { get; init; } = default!;
}

/// <summary>
/// Event raised when benefit enrollment is terminated.
/// </summary>
public record BenefitEnrollmentTerminated : DomainEvent
{
    public BenefitEnrollment Enrollment { get; init; } = default!;
}

