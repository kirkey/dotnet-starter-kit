using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralTypes;

public class CollateralTypeViewModel : IEntity<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal DefaultLtvPercent { get; set; }
    public decimal MaxLtvPercent { get; set; }
    public int DefaultUsefulLifeYears { get; set; }
    public decimal AnnualDepreciationRate { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CollateralTypeResponse, CollateralTypeViewModel>();
            config.NewConfig<CollateralTypeViewModel, CreateCollateralTypeCommand>();
        }
    }
}
