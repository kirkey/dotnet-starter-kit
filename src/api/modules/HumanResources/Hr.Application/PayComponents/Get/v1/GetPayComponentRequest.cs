namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;

public sealed record GetPayComponentRequest(DefaultIdType Id) : IRequest<PayComponentResponse>;