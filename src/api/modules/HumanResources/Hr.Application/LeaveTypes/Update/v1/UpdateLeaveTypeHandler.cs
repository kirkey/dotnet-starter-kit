namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

/// <summary>
/// Handler for updating leave type with Philippines Labor Code compliance.
/// Supports partial updates - only provided fields will be updated.
/// </summary>
public sealed class UpdateLeaveTypeHandler(
    ILogger<UpdateLeaveTypeHandler> logger,
    [FromKeyedServices("hr:leavetypes")] IRepository<LeaveType> repository)
    : IRequestHandler<UpdateLeaveTypeCommand, UpdateLeaveTypeResponse>
{
    public async Task<UpdateLeaveTypeResponse> Handle(
        UpdateLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        var leaveType = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.Id);

        // Update basic configuration
        if (!string.IsNullOrWhiteSpace(request.LeaveName) || request.AnnualAllowance.HasValue ||
            !string.IsNullOrWhiteSpace(request.AccrualFrequency) || request.IsPaid.HasValue ||
            request.RequiresApproval.HasValue || !string.IsNullOrWhiteSpace(request.Description))
        {
            leaveType.Update(
                leaveName: request.LeaveName,
                annualAllowance: request.AnnualAllowance,
                accrualFrequency: request.AccrualFrequency,
                isPaid: request.IsPaid,
                requiresApproval: request.RequiresApproval,
                description: request.Description);
        }

        // Update carryover policy
        if (request.MaxCarryoverDays.HasValue)
            leaveType.SetCarryoverPolicy(request.MaxCarryoverDays.Value, request.CarryoverExpiryMonths);

        // Update minimum notice
        if (request.MinimumNoticeDay.HasValue)
            leaveType.SetMinimumNotice(request.MinimumNoticeDay.Value);

        // Philippines-Specific: Update leave code
        if (!string.IsNullOrWhiteSpace(request.LeaveCode))
            leaveType.SetLeaveCode(request.LeaveCode);

        // Philippines-Specific: Update gender applicability
        if (!string.IsNullOrWhiteSpace(request.ApplicableGender))
            leaveType.SetApplicableGender(request.ApplicableGender);

        // Philippines-Specific: Update minimum service days
        if (request.MinimumServiceDays.HasValue)
            leaveType.SetMinimumServiceDays(request.MinimumServiceDays.Value);

        // Philippines-Specific: Update medical certification requirement
        if (request.RequiresMedicalCertification.HasValue)
            leaveType.SetMedicalCertificationRequirement(
                request.RequiresMedicalCertification.Value,
                request.MedicalCertificateAfterDays ?? 0);

        // Philippines-Specific: Update cash convertibility
        if (request.IsConvertibleToCash.HasValue)
            leaveType.SetCashConvertibility(request.IsConvertibleToCash.Value);

        // Philippines-Specific: Update cumulative property
        if (request.IsCumulative.HasValue)
            leaveType.SetCumulative(request.IsCumulative.Value);

        // Update active status
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                leaveType.Activate();
            else
                leaveType.Deactivate();
        }

        await repository.UpdateAsync(leaveType, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Leave type {LeaveTypeId} updated successfully. Code: {LeaveCode}, Convertible: {Convertible}, Cumulative: {Cumulative}",
            leaveType.Id,
            leaveType.LeaveCode,
            leaveType.IsConvertibleToCash,
            leaveType.IsCumulative);

        return new UpdateLeaveTypeResponse(leaveType.Id);
    }
}

