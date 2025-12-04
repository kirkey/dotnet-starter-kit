using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;

public sealed record GetLoanOfficerAssignmentRequest(Guid Id) : IRequest<LoanOfficerAssignmentResponse>;
