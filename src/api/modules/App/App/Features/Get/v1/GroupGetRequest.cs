using MediatR;

namespace FSH.Starter.WebApi.App.Features.Get.v1;

public class GetAppRequest(DefaultIdType id) : IRequest<GroupGetResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
