using Accounting.Domain.Events.Customer;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Customer : AuditableEntity, IAggregateRoot
{
    public string CustomerCode { get; private set; }
    public string? Address { get; private set; }
    public string? BillingAddress { get; private set; }
    public string? ContactPerson { get; private set; }
    public string? Email { get; private set; }
    public string? Terms { get; private set; }
    public string? RevenueAccountCode { get; private set; }
    public string? RevenueAccountName { get; private set; }
    public string? Tin { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal CurrentBalance { get; private set; }

    private Customer(string customerCode, string name, string? address, string? billingAddress,
        string? contactPerson, string? email, string? terms, string? revenueAccountCode, string? revenueAccountName,
        string? tin, string? phoneNumber, decimal creditLimit, string? description, string? notes)
    {
        CustomerCode = customerCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        BillingAddress = billingAddress?.Trim();
        ContactPerson = contactPerson?.Trim();
        Email = email?.Trim();
        Terms = terms?.Trim();
        RevenueAccountCode = revenueAccountCode?.Trim();
        RevenueAccountName = revenueAccountName?.Trim();
        Tin = tin?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        CreditLimit = creditLimit;
        CurrentBalance = 0;
        Description = description?.Trim();
        Notes = notes?.Trim();
        IsActive = true;

        QueueDomainEvent(new CustomerCreated(Id, CustomerCode, Name, Email, Terms, CreditLimit, Description, Notes));
    }

    public static Customer Create(string customerCode, string name, string? address = null, string? billingAddress = null,
        string? contactPerson = null, string? email = null, string? terms = null, string? revenueAccountCode = null,
        string? revenueAccountName = null, string? tin = null, string? phoneNumber = null, decimal creditLimit = 0,
        string? description = null, string? notes = null)
    {
        if (creditLimit < 0)
            throw new InvalidCustomerCreditLimitException();

        return new Customer(customerCode, name, address, billingAddress, contactPerson,
            email, terms, revenueAccountCode, revenueAccountName, tin, phoneNumber, creditLimit, description, notes);
    }

    public Customer Update(string? customerCode, string? name, string? address, string? billingAddress,
        string? contactPerson, string? email, string? terms, string? revenueAccountCode, string? revenueAccountName,
        string? tin, string? phoneNumber, decimal? creditLimit, string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(customerCode) && !string.Equals(CustomerCode, customerCode, StringComparison.OrdinalIgnoreCase))
        {
            CustomerCode = customerCode.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }

        if (address != Address)
        {
            Address = address?.Trim();
            isUpdated = true;
        }

        if (billingAddress != BillingAddress)
        {
            BillingAddress = billingAddress?.Trim();
            isUpdated = true;
        }

        if (contactPerson != ContactPerson)
        {
            ContactPerson = contactPerson?.Trim();
            isUpdated = true;
        }

        if (email != Email)
        {
            Email = email?.Trim();
            isUpdated = true;
        }

        if (terms != Terms)
        {
            Terms = terms?.Trim();
            isUpdated = true;
        }

        if (revenueAccountCode != RevenueAccountCode)
        {
            RevenueAccountCode = revenueAccountCode?.Trim();
            isUpdated = true;
        }

        if (revenueAccountName != RevenueAccountName)
        {
            RevenueAccountName = revenueAccountName?.Trim();
            isUpdated = true;
        }

        if (tin != Tin)
        {
            Tin = tin?.Trim();
            isUpdated = true;
        }

        if (phoneNumber != PhoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
            isUpdated = true;
        }

        if (creditLimit.HasValue && CreditLimit != creditLimit.Value)
        {
            if (creditLimit.Value < 0)
                throw new InvalidCustomerCreditLimitException();
            CreditLimit = creditLimit.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CustomerUpdated(this));
        }

        return this;
    }

    public Customer AddToBalance(decimal amount, string transactionType, string? reference = null)
    {
        if (amount <= 0)
            throw new InvalidCustomerBalanceTransactionException();

        CurrentBalance += amount;
        QueueDomainEvent(new CustomerBalanceChanged(Id, CurrentBalance, amount, transactionType, reference));
        return this;
    }

    public Customer ReduceBalance(decimal amount, string transactionType, string? reference = null)
    {
        if (amount <= 0)
            throw new InvalidCustomerBalanceTransactionException();

        CurrentBalance -= amount;
        QueueDomainEvent(new CustomerBalanceChanged(Id, CurrentBalance, -amount, transactionType, reference));
        return this;
    }

    public Customer SetCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit < 0)
            throw new InvalidCustomerCreditLimitException();

        var oldLimit = CreditLimit;
        CreditLimit = newCreditLimit;
        QueueDomainEvent(new CustomerCreditLimitChanged(Id, oldLimit, newCreditLimit));
        return this;
    }

    public Customer Activate()
    {
        if (IsActive)
            throw new CustomerAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new CustomerActivated(Id, CustomerCode, Name));
        return this;
    }

    public Customer Deactivate()
    {
        if (!IsActive)
            throw new CustomerAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new CustomerDeactivated(Id, CustomerCode, Name));
        return this;
    }

    public bool IsCreditLimitExceeded()
    {
        return CurrentBalance > CreditLimit;
    }

    public decimal GetAvailableCredit()
    {
        return Math.Max(0, CreditLimit - CurrentBalance);
    }

    public bool CanProcessOrder(decimal orderAmount)
    {
        if ((CurrentBalance + orderAmount) > CreditLimit)
            throw new CustomerCreditLimitExceededException(Id, CurrentBalance, CreditLimit);
            
        return (CurrentBalance + orderAmount) <= CreditLimit;
    }
}
