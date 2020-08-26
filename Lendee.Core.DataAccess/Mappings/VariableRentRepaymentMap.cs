using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class VariableRentRepaymentMap : IEntityTypeConfiguration<VariableRentRepayment>
    {
        public void Configure(EntityTypeBuilder<VariableRentRepayment> builder)
        {
            builder.Property(x => x.UnitPrice).HasColumnName("amount");
            builder.Property(x => x.Amount).HasColumnName("fee");
        }
    }
}