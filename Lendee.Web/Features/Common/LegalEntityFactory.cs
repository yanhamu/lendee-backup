using Lendee.Core.Domain.Model;
using Lendee.Web.Features.Entity;

namespace Lendee.Web.Features.Common
{
    public class LegalEntityFactory
    {
        public LegalEntity Create(EntitiesController.EntityViewModel model)
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

        public EntitiesController.EntityViewModel Create(LegalEntity entity)
        {
            return new EntitiesController.EntityViewModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                BankAccountNumber = entity.BankAccountNumber,
                Note = entity.Note,
                CompanyName = entity.CompanyName,
                IdentifyingNumber = entity.IdentifyingNumber,
                TaxIdentifyingNumber = entity.TaxIdentifyingNumber,
                Id = entity.Id
            };
        }
    }
}
