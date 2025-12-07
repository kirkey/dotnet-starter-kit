namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the StaffTraining entity.
/// </summary>
internal sealed class StaffTrainingConfiguration : IEntityTypeConfiguration<StaffTraining>
{
    public void Configure(EntityTypeBuilder<StaffTraining> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrainingCode)
            .HasMaxLength(StaffTraining.MaxLengths.TrainingCode);

        builder.Property(x => x.TrainingName)
            .HasMaxLength(StaffTraining.MaxLengths.TrainingName);

        builder.Property(x => x.Description)
            .HasMaxLength(StaffTraining.MaxLengths.Description);

        builder.Property(x => x.TrainingType)
            .HasMaxLength(StaffTraining.MaxLengths.TrainingType);

        builder.Property(x => x.Provider)
            .HasMaxLength(StaffTraining.MaxLengths.Provider);

        builder.Property(x => x.Location)
            .HasMaxLength(StaffTraining.MaxLengths.Location);

        builder.Property(x => x.Score)
            .HasPrecision(18, 2);

        builder.Property(x => x.PassingScore)
            .HasPrecision(18, 2);

        builder.Property(x => x.CertificationNumber)
            .HasMaxLength(StaffTraining.MaxLengths.CertificationNumber);

        builder.Property(x => x.TrainingCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(StaffTraining.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Staff)
            .WithMany(x => x.Trainings)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.StaffId);
        builder.HasIndex(x => x.Status);
    }
}
