using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    public class PaymentDueMap : IEntityTypeConfiguration<PaymentDue>
    {
        public void Configure(EntityTypeBuilder<PaymentDue> builder)
        {
            builder.ToTable("payment_dues");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("decimal(19,3)"); ;
            builder.Property(x => x.ContractId).HasColumnName("contract_id");
            builder.Property(x => x.Due).HasColumnName("due");
        }
    }
}