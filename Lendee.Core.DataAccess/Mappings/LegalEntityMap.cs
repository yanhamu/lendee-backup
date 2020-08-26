using Lendee.Core.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendee.Core.DataAccess.Mappings
{
    internal class LegalEntityMap : IEntityTypeConfiguration<LegalEntity>
    {
        public void Configure(EntityTypeBuilder<LegalEntity> builder)
        {
            builder.ToTable("legal_entities");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).HasColumnName("firstname");
            builder.Property(x => x.LastName).HasColumnName("lastname");
            builder.Property(x => x.CompanyName).HasColumnName("company_name");

            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.PhoneNumber).HasColumnName("phone");
            builder.Property(x => x.BankAccountNumber).HasColumnName("bank_account_number");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.IdentifyingNumber).HasColumnName("identifying_number");
            builder.Property(x => x.TaxIdentifyingNumber).HasColumnName("tax_identifying_number");
        }
    }
}
