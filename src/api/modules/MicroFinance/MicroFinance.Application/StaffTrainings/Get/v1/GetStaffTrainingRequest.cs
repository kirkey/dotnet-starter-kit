using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;

/// <summary>
/// Request to get a staff training by ID.
/// </summary>
public sealed record GetStaffTrainingRequest(DefaultIdType Id) : IRequest<StaffTrainingResponse>;
