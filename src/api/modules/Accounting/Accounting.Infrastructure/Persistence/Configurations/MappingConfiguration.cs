using Accounting.Application.Accounts.Dtos;
using Accounting.Domain;
using Mapster;

namespace Accounting.Infrastructure.Persistence.Configurations;

public static class MappingConfiguration
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Account, AccountDto>.NewConfig()
            .ConstructUsing(src => new AccountDto());
    }
}
