using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    public class ContractMap : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("contracts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Type).HasColumnName("type");
            builder.Property(x => x.Currency).HasColumnName("currency");
            builder.Property(x => x.LenderId).HasColumnName("lender");
            builder.Property(x => x.LendeeId).HasColumnName("lendee");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.PaymentTermType).HasColumnName("payment_term_type");
            builder.Property(x => x.ValidFrom).HasColumnName("valid_from");
            builder.Property(x => x.ValidUntil).HasColumnName("valid_until");
            builder.HasOne(x => x.Lendee).WithMany().HasForeignKey(x => x.LendeeId).IsRequired(false);
            builder.HasOne(x => x.Lender).WithMany().HasForeignKey(x => x.LenderId).IsRequired(false);
            builder.HasDiscriminator(x => x.Type)
                .HasValue<Contract>(ContractType.Undefined)
                .HasValue<CombinedRent>(ContractType.CombinedRent)
                .HasValue<VariableRent>(ContractType.VariableRent)
                .HasValue<Rent>(ContractType.Rent)
                .HasValue<Loan>(ContractType.Loan)
                .HasValue<LoanWithInterest>(ContractType.LoanWithInterest);

            builder.OwnsOne(
                x => x.PaymentTermData,
                p =>
                {
                    p.Property(d => d.Day).HasColumnName("term_day");
                    p.Property(d => d.Month).HasColumnName("term_month");
                });

        }
    }
}
