using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;

public sealed record GetStaffRequest(Guid Id) : IRequest<StaffResponse>;
