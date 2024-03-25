using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationEntity = Domain.Entities.Application;

namespace Persistence.EntityConfigurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
    {
        builder.ToTable("Applications").HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("Id").IsRequired();
        builder.Property(a => a.ApplicantId).HasColumnName("ApplicantId");
        builder.Property(a => a.BootcampId).HasColumnName("BootcampId");
        builder.Property(a => a.ApplicationStateId).HasColumnName("ApplicationStateId");
        builder.Property(a => a.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(a => a.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(a => a.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(a => !a.DeletedDate.HasValue);

        builder.HasOne(x => x.Applicant).WithMany().HasForeignKey(x => x.ApplicantId);
        builder.HasOne(x => x.Bootcamp).WithMany().HasForeignKey(x => x.BootcampId);
        builder.HasOne(x => x.ApplicationState).WithMany().HasForeignKey(x => x.ApplicationStateId);
    }
}
