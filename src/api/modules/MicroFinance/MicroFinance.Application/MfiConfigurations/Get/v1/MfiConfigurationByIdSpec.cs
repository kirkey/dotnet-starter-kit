using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MfiConfigurations.Get.v1;

public sealed class MfiConfigurationByIdSpec : Specification<MfiConfiguration>
{
    public MfiConfigurationByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}

public sealed class MfiConfigurationByKeySpec : Specification<MfiConfiguration>
{
    public MfiConfigurationByKeySpec(string key)
    {
        Query.Where(x => x.Key == key);
    }
}
