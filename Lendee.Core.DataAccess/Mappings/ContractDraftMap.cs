using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class ContractDraftMap : IEntityTypeConfiguration<ContractDraft>
    {
        public void Configure(EntityTypeBuilder<ContractDraft> builder)
        {
            builder.ToTable("contract_drafts");
            builder.HasKey(x => x.ContractId);
            builder.Property(x => x.ContractId).HasColumnName("contract_id");
            builder.Property(x => x.Step).HasColumnName("step");
            builder.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);
        }
    }
}
