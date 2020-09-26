using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("decimal(11,3)");
            builder.Property(x => x.ReceivedAt).HasColumnName("received_at");
            builder.Property(x => x.ContractId).HasColumnName("contract_id");
            builder.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);
        }
    }
}