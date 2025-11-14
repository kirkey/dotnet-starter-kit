namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

/// <summary>
/// Handler for creating a new leave type with Philippines Labor Code compliance.
/// Sets all mandatory Philippines-specific fields including gender rules and medical certification.
/// </summary>
public sealed class CreateLeaveTypeHandler(
    ILogger<CreateLeaveTypeHandler> logger,
    [FromKeyedServices("hr:leavetypes")] IRepository<LeaveType> repository)
    : IRequestHandler<CreateLeaveTypeCommand, CreateLeaveTypeResponse>
{
    public async Task<CreateLeaveTypeResponse> Handle(
        CreateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create leave type with basic information
        var leaveType = LeaveType.Create(
            request.LeaveName,
            request.AnnualAllowance,
            request.IsPaid,
            request.RequiresApproval);

        // Set basic configuration
        leaveType.Update(accrualFrequency: request.AccrualFrequency, description: request.Description);

        // Set carryover policy
        if (request.MaxCarryoverDays > 0)
            leaveType.SetCarryoverPolicy(request.MaxCarryoverDays, request.CarryoverExpiryMonths);

        // Set minimum notice requirement
        if (request.MinimumNoticeDay.HasValue)
            leaveType.SetMinimumNotice(request.MinimumNoticeDay.Value);

        // Philippines-Specific: Set leave code classification
        if (!string.IsNullOrWhiteSpace(request.LeaveCode))
            leaveType.SetLeaveCode(request.LeaveCode);

        // Philippines-Specific: Set gender applicability (for maternity/paternity)
        leaveType.SetApplicableGender(request.ApplicableGender);

        // Philippines-Specific: Set minimum service requirement
        if (request.MinimumServiceDays > 0)
            leaveType.SetMinimumServiceDays(request.MinimumServiceDays);

        // Philippines-Specific: Set medical certification requirement
        if (request.RequiresMedicalCertification)
            leaveType.SetMedicalCertificationRequirement(
                request.RequiresMedicalCertification,
                request.MedicalCertificateAfterDays);

        // Philippines-Specific: Set cash convertibility (Vacation: Yes, Sick: No)
        leaveType.SetCashConvertibility(request.IsConvertibleToCash);

        // Philippines-Specific: Set cumulative property (Vacation: Yes, Sick: No)
        leaveType.SetCumulative(request.IsCumulative);

        await repository.AddAsync(leaveType, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave type created: ID {LeaveTypeId}, Name {LeaveName}, Code {LeaveCode}, " +
            "Annual Allowance {AnnualAllowance} days, Gender {Gender}, Convertible {Convertible}, Cumulative {Cumulative}",
            leaveType.Id,
            request.LeaveName,
            request.LeaveCode,
            request.AnnualAllowance,
            request.ApplicableGender,
            request.IsConvertibleToCash,
            request.IsCumulative);

        return new CreateLeaveTypeResponse(leaveType.Id);
    }
}

