using FSH.Starter.WebApi.Store.Application.Customers.Get.v1;


namespace FSH.Starter.WebApi.Store.Application.Customers.Search.v1;

public class SearchCustomersSpecs : EntitiesByPaginationFilterSpec<Customer, CustomerResponse>
{
    public SearchCustomersSpecs(SearchCustomersCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(c => c.Name!.Contains(command.Name!), !string.IsNullOrEmpty(command.Name))
            .Where(c => c.Code == command.Code, !string.IsNullOrEmpty(command.Code))
            .Where(c => c.CustomerType == command.CustomerType, !string.IsNullOrEmpty(command.CustomerType))
            .Where(c => c.Email!.Contains(command.Email!), !string.IsNullOrEmpty(command.Email))
            .Where(c => c.City == command.City, !string.IsNullOrEmpty(command.City))
            .Where(c => c.Country == command.Country, !string.IsNullOrEmpty(command.Country));
}
