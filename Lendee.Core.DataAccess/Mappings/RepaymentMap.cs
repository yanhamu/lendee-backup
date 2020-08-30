using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class RepaymentMap : IEntityTypeConfiguration<Repayment>
    {
        public void Configure(EntityTypeBuilder<Repayment> builder)
        {
            builder.ToTable("repayments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DueDate).HasColumnName("due_date");
            builder.Property(x => x.ContractId).HasColumnName("contract_id");
            builder.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);

            builder.HasDiscriminator<ContractType>("contract_type")
                .HasValue<RentRepayment>(ContractType.Rent)
                .HasValue<CombinedRentRepayment>(ContractType.CombinedRent)
                .HasValue<VariableRentRepayment>(ContractType.VariableRent)
                .HasValue<LoanRepayment>(ContractType.Loan)
                .HasValue<LoanWithInterestRepayment>(ContractType.LoanWithInterest);

        }
    }
}
