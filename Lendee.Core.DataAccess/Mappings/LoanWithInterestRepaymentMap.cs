using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    public class LoanWithInterestRepaymentMap : IEntityTypeConfiguration<LoanWithInterestRepayment>
    {
        public void Configure(EntityTypeBuilder<LoanWithInterestRepayment> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("amount");
            builder.Property(x => x.Interest).HasColumnName("fee");
        }
    }
}
