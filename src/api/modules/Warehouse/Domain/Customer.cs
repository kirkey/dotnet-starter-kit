using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Customer : AuditableEntity, IAggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateTime? DateOfBirth { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LoyaltyPoints { get; private set; } = 0;
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSpent { get; private set; } = 0;

    public ICollection<Sale> Sales { get; private set; } = new List<Sale>();

    private Customer() { }

    public static Customer Create(string firstName, string lastName, string phone, string email, string address, DateTime? dateOfBirth)
    {
        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Email = email,
            Address = address,
            DateOfBirth = dateOfBirth
        };
    }

    public Customer Update(string? firstName, string? lastName, string? phone, string? email, string? address, DateTime? dateOfBirth, decimal? loyaltyPoints, decimal? totalSpent)
    {
        if (firstName is not null) FirstName = firstName;
        if (lastName is not null) LastName = lastName;
        if (phone is not null) Phone = phone;
        if (email is not null) Email = email;
        if (address is not null) Address = address;
        if (dateOfBirth.HasValue) DateOfBirth = dateOfBirth.Value;
        if (loyaltyPoints.HasValue) LoyaltyPoints = loyaltyPoints.Value;
        if (totalSpent.HasValue) TotalSpent = totalSpent.Value;
        return this;
    }
}

