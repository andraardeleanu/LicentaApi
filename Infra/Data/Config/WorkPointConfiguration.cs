using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infra.Data.Config
{
    public class WorkpointConfiguration : IEntityTypeConfiguration<WorkPoint>
    {
        public void Configure(EntityTypeBuilder<WorkPoint> builder)
        {
            builder.HasKey(t => t.Id);                
        }
    }
}
