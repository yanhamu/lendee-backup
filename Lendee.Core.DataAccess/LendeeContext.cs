using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Lendee.Core.DataAccess
{
    public class LendeeContext : DbContext
    {
        public LendeeContext(DbContextOptions<LendeeContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("core");

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
        }
    }
}