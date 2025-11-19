using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(b => b.Id);

        builder.Property(b => b.AccountNumber)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.RoutingNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.BankName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.AccountType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.AccountHolderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Last4Digits)
            .HasMaxLength(4);

        builder.Property(b => b.SwiftCode)
            .HasMaxLength(50);

        builder.Property(b => b.Iban)
            .HasMaxLength(50);

        builder.Property(b => b.CurrencyCode)
            .HasMaxLength(10);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.HasOne(b => b.Employee)
            .WithMany()
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(b => b.EmployeeId)
            .HasDatabaseName("IX_BankAccount_EmployeeId");

        builder.HasIndex(b => new { b.EmployeeId, b.IsPrimary })
            .HasDatabaseName("IX_BankAccount_Employee_Primary");

        builder.HasIndex(b => b.IsActive)
            .HasDatabaseName("IX_BankAccount_IsActive");

        builder.HasIndex(b => b.IsVerified)
            .HasDatabaseName("IX_BankAccount_IsVerified");

        builder.HasIndex(b => b.AccountType)
            .HasDatabaseName("IX_BankAccount_AccountType");

        builder.HasIndex(b => b.Last4Digits)
            .HasDatabaseName("IX_BankAccount_Last4Digits");
    }
}

