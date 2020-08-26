using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class RentMap : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("payment_amount").IsRequired();
        }
    }
}
