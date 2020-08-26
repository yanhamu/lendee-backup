using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    public class RentRepaymentMap : IEntityTypeConfiguration<RentRepayment>
    {
        public void Configure(EntityTypeBuilder<RentRepayment> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("amount");
        }
    }
}