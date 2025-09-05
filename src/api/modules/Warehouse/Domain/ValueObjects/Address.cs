namespace FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

public record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country)
{
    public override string ToString() =>
        $"{Street}, {City}, {State} {PostalCode}, {Country}";
}
