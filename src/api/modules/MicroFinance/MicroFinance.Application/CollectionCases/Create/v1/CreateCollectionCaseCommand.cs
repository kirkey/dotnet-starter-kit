using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;

public sealed record CreateCollectionCaseCommand(
    [property: DefaultValue("CC-20241204-001")] string CaseNumber,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid LoanId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid MemberId,
    [property: DefaultValue(30)] int DaysPastDue,
    [property: DefaultValue(5000)] decimal AmountOverdue,
    [property: DefaultValue(50000)] decimal TotalOutstanding)
    : IRequest<CreateCollectionCaseResponse>;
