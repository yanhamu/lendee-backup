using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Lendee.Core.DataAccess
{
    public class LendeeContext : DbContext
    {
        public LendeeContext(DbContextOptions<LendeeContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lendee");

            var legalEntity = modelBuilder.Entity<LegalEntity>();
            legalEntity.ToTable("legal_entities");
            legalEntity.HasKey(x => x.Id);
            legalEntity.Property(x => x.FirstName).HasColumnName("firstname");
            legalEntity.Property(x => x.LastName).HasColumnName("lastname");
            legalEntity.Property(x => x.CompanyName).HasColumnName("company_name");

            legalEntity.Property(x => x.Email).HasColumnName("email");
            legalEntity.Property(x => x.PhoneNumber).HasColumnName("phone");
            legalEntity.Property(x => x.BankAccountNumber).HasColumnName("bank_account_number");
            legalEntity.Property(x => x.Note).HasColumnName("note");
            legalEntity.Property(x => x.IdentifyingNumber).HasColumnName("identifying_number");
            legalEntity.Property(x => x.TaxIdentifyingNumber).HasColumnName("tax_identifying_number");

            var contract = modelBuilder.Entity<Contract>();
            contract.ToTable("contracts");
            contract.HasKey(x => x.Id);
            contract.Property(x => x.Name).HasColumnName("name");
            contract.Property(x => x.Type).HasColumnName("type");
            contract.Property(x => x.Currency).HasColumnName("currency");
            contract.Property(x => x.LenderId).HasColumnName("lender");
            contract.Property(x => x.LendeeId).HasColumnName("lendee");
            contract.Property(x => x.Note).HasColumnName("note");
            contract.Property(x => x.PaymentTermType).HasColumnName("payment_term_type");
            contract.Property(x => x.ValidFrom).HasColumnName("valid_from");
            contract.Property(x => x.ValidUntil).HasColumnName("valid_until");
            contract.HasOne(x => x.Lendee).WithMany().HasForeignKey(x => x.LendeeId).IsRequired(false);
            contract.HasOne(x => x.Lender).WithMany().HasForeignKey(x => x.LenderId).IsRequired(false);
            contract.HasDiscriminator(x => x.Type)
                .HasValue<Contract>(ContractType.Undefined)
                .HasValue<CombinedRent>(ContractType.CombinedRent)
                .HasValue<VariableRent>(ContractType.VariableRent)
                .HasValue<Rent>(ContractType.Rent)
                .HasValue<Credit>(ContractType.Credit);
            contract.OwnsOne(
                x => x.PaymentTermData,
                p =>
                {
                    p.Property(d => d.Day).HasColumnName("term_day");
                    p.Property(d => d.Month).HasColumnName("term_month");
                });

            var combinedRent = modelBuilder.Entity<CombinedRent>();
            combinedRent.Property(x => x.Amount).HasColumnName("payment_amount").IsRequired();
            combinedRent.Property(x => x.Fee).HasColumnName("fee").IsRequired();

            var variableRent = modelBuilder.Entity<VariableRent>();
            variableRent.Property(x => x.UnitPrice).HasColumnName("payment_amount").IsRequired();

            var rent = modelBuilder.Entity<Rent>();
            rent.Property(x => x.Amount).HasColumnName("payment_amount").IsRequired();

            var credit = modelBuilder.Entity<Credit>();
            credit.Property(x => x.InterestRate).HasColumnName("interest_rate");
            credit.Property(x => x.PrincipalSum).HasColumnName("principal_sum");

            var repayment = modelBuilder.Entity<Repayment>();
            repayment.ToTable("repayments");
            repayment.HasKey(x => x.Id);
            repayment.Property(x => x.DueDate).HasColumnName("due_date");
            repayment.Property(x => x.ContractId).HasColumnName("contract_id");
            repayment.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);

            repayment.HasDiscriminator<ContractType>("contract_type")
                .HasValue<RentRepayment>(ContractType.Rent)
                .HasValue<CombinedRentRepayment>(ContractType.CombinedRent)
                .HasValue<VariableRentRepayment>(ContractType.VariableRent);

            var rentRepayment = modelBuilder.Entity<RentRepayment>()
                .Property(x => x.Amount).HasColumnName("amount");

            var combinedRentRepayment = modelBuilder.Entity<CombinedRentRepayment>();
            combinedRentRepayment.Property(x => x.Amount).HasColumnName("amount");
            combinedRentRepayment.Property(x => x.Fee).HasColumnName("fee");

            var variableRentRepayment = modelBuilder.Entity<VariableRentRepayment>();
            variableRentRepayment.Property(x => x.UnitPrice).HasColumnName("amount");
            variableRentRepayment.Property(x => x.Amount).HasColumnName("fee");

            var payment = modelBuilder.Entity<Payment>();
            payment.ToTable("payments");
            payment.HasKey(x => x.Id);
            payment.Property(x => x.Amount).HasColumnName("amount");
            payment.Property(x => x.DueDate).HasColumnName("due");
            payment.Property(x => x.ContractId).HasColumnName("contract_id");
            payment.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);

            var contractDraft = modelBuilder.Entity<ContractDraft>();
            contractDraft.ToTable("contract_drafts");
            contractDraft.HasKey(x => x.ContractId);
            contractDraft.Property(x => x.ContractId).HasColumnName("contract_id");
            contractDraft.Property(x => x.Step).HasColumnName("step");
            contractDraft.HasOne(x => x.Contract).WithMany().HasForeignKey(x => x.ContractId);
        }
    }
}