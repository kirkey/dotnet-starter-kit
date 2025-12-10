namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;

public sealed record GetStaffRequest(DefaultIdType Id) : IRequest<StaffResponse>;
