namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the GroupMembership entity.
/// </summary>
internal sealed class GroupMembershipConfiguration : IEntityTypeConfiguration<GroupMembership>
{
    public void Configure(EntityTypeBuilder<GroupMembership> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasMaxLength(64);

        builder.Property(x => x.Status)
            .HasMaxLength(32);

        // Relationships
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MemberGroup>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_GroupMemberships_MemberId");

        builder.HasIndex(x => x.GroupId)
            .HasDatabaseName("IX_GroupMemberships_GroupId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_GroupMemberships_Status");
    }
}

