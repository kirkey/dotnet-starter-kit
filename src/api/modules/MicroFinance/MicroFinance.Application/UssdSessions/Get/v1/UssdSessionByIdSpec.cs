using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;

public sealed class UssdSessionByIdSpec : Specification<UssdSession>
{
    public UssdSessionByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

public sealed class UssdSessionBySessionIdSpec : Specification<UssdSession>
{
    public UssdSessionBySessionIdSpec(string sessionId)
    {
        Query.Where(x => x.SessionId == sessionId);
    }
}
