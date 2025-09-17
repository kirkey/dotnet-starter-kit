using Accounting.Application.ChartOfAccounts.Dtos;
using Mapster;

namespace Accounting.Infrastructure.Persistence.Configurations;

public static class MappingConfiguration
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<ChartOfAccount, ChartOfAccountDto>.NewConfig()
            .ConstructUsing(src => new ChartOfAccountDto());
    }
}
