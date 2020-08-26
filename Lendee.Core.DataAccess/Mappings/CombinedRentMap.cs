using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class CombinedRentMap : IEntityTypeConfiguration<CombinedRent>
    {
        public void Configure(EntityTypeBuilder<CombinedRent> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("payment_amount").IsRequired();
            builder.Property(x => x.Fee).HasColumnName("fee").IsRequired();
        }
    }
}