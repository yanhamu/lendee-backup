using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class LoanRepaymentMap : IEntityTypeConfiguration<LoanRepayment>
    {
        public void Configure(EntityTypeBuilder<LoanRepayment> builder)
        {
            builder.Property(x => x.Amount).HasColumnName("amount");
        }
    }
}
