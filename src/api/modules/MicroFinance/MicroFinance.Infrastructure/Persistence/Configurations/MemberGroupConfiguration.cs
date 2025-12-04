namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MemberGroup entity.
/// </summary>
internal sealed class MemberGroupConfiguration : IEntityTypeConfiguration<MemberGroup>
{
    public void Configure(EntityTypeBuilder<MemberGroup> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(MemberGroup.CodeMaxLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MemberGroup.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(MemberGroup.DescriptionMaxLength);

        builder.Property(x => x.MeetingLocation)
            .HasMaxLength(MemberGroup.MeetingLocationMaxLength);

        builder.Property(x => x.MeetingFrequency)
            .HasMaxLength(MemberGroup.MeetingFrequencyMaxLength);

        builder.Property(x => x.MeetingDay)
            .HasMaxLength(MemberGroup.MeetingDayMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(MemberGroup.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(MemberGroup.NotesMaxLength);

        // Relationships
        builder.HasOne(x => x.Leader)
            .WithMany()
            .HasForeignKey(x => x.LeaderMemberId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_MemberGroups_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_MemberGroups_Name");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_MemberGroups_Status");

        builder.HasIndex(x => x.LeaderMemberId)
            .HasDatabaseName("IX_MemberGroups_LeaderMemberId");

        builder.HasIndex(x => x.LoanOfficerId)
            .HasDatabaseName("IX_MemberGroups_LoanOfficerId");
    }
}
