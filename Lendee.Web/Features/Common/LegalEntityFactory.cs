using Lendee.Core.Domain.Model;
using Lendee.Web.Features.Entity;

namespace Lendee.Web.Features.Common
{
    public class LegalEntityFactory
    {
        public LegalEntity Create(EntityViewModel model)
        {
            return new LegalEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankAccountNumber = model.BankAccountNumber,
                Note = model.Note,
                CompanyName = model.CompanyName,
                IdentifyingNumber = model.IdentifyingNumber,
                TaxIdentifyingNumber = model.TaxIdentifyingNumber
            };
        }
    }
}
