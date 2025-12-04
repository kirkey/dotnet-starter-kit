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
            .IsRequired()
            .HasMaxLength(GroupMembership.RoleMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(GroupMembership.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(GroupMembership.NotesMaxLength);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Group)
            .WithMany(g => g.Memberships)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => new { x.MemberId, x.GroupId })
            .IsUnique()
            .HasDatabaseName("IX_GroupMemberships_MemberGroup");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_GroupMemberships_MemberId");

        builder.HasIndex(x => x.GroupId)
            .HasDatabaseName("IX_GroupMemberships_GroupId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_GroupMemberships_Status");

        builder.HasIndex(x => x.JoinDate)
            .HasDatabaseName("IX_GroupMemberships_JoinDate");
    }
}
