using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class CombinedRentRepaymentMap : IEntityTypeConfiguration<CombinedRentRepayment>
    {
        public void Configure(EntityTypeBuilder<CombinedRentRepayment> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("amount");
            builder.Property(x => x.Fee).HasColumnName("fee");
        }
    }
}
