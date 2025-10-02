using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(x => x.Id);
        // ...additional configuration...
    }
}

