using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Get.v1;

public sealed record GetStaffRequest(DefaultIdType Id) : IRequest<StaffResponse>;
