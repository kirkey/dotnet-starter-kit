using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;

public sealed record DeletePayrollDeductionCommand(DefaultIdType Id) : IRequest<DeletePayrollDeductionResponse>;

