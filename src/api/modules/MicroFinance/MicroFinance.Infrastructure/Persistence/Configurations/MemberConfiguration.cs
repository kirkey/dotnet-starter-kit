namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Member entity.
/// </summary>
internal sealed class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MemberNumber)
            .IsRequired()
            .HasMaxLength(Member.MemberNumberMaxLength);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(Member.FirstNameMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(Member.LastNameMaxLength);

        builder.Property(x => x.MiddleName)
            .HasMaxLength(Member.MiddleNameMaxLength);

        builder.Property(x => x.Email)
            .HasMaxLength(Member.EmailMaxLength);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(Member.PhoneNumberMaxLength);

        builder.Property(x => x.Gender)
            .HasMaxLength(Member.GenderMaxLength);

        builder.Property(x => x.Address)
            .HasMaxLength(Member.AddressMaxLength);

        builder.Property(x => x.NationalId)
            .HasMaxLength(Member.NationalIdMaxLength);

        builder.Property(x => x.Occupation)
            .HasMaxLength(Member.OccupationMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(Member.NotesMaxLength);

        builder.Property(x => x.MonthlyIncome)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.MemberNumber)
            .IsUnique()
            .HasDatabaseName("IX_Members_MemberNumber");

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_Members_Email");

        builder.HasIndex(x => x.NationalId)
            .HasDatabaseName("IX_Members_NationalId");

        builder.HasIndex(x => new { x.LastName, x.FirstName })
            .HasDatabaseName("IX_Members_Name");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_Members_IsActive");
    }
}

