using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class VariableRentMap : IEntityTypeConfiguration<VariableRent>
    {
        public void Configure(EntityTypeBuilder<VariableRent> builder)
        {
            builder.Property(x => x.UnitPrice).HasColumnName("payment_amount").IsRequired();
        }
    }
}
