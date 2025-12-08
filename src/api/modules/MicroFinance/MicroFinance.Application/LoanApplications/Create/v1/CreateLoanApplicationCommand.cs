using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Create.v1;

/// <summary>
/// Command to create a new loan application.
/// </summary>
public sealed record CreateLoanApplicationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ProductId,
    [property: DefaultValue(100000)] decimal RequestedAmount,
    [property: DefaultValue(12)] int RequestedTermMonths,
    [property: DefaultValue("Business Expansion")] string Purpose,
    [property: DefaultValue(null)] DefaultIdType? GroupId = null
) : IRequest<CreateLoanApplicationResponse>;
