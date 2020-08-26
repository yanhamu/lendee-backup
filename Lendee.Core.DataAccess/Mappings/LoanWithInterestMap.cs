using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class LoanWithInterestMap : IEntityTypeConfiguration<LoanWithInterest>
    {
        public void Configure(EntityTypeBuilder<LoanWithInterest> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("payment_amount");
            builder.Property(x => x.InterestRate).HasColumnName("interest_rate");
        }
    }
}
