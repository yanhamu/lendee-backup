using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lendee.Web.Features.Entity
{
    public class EntitiesController : Controller
    {
        private readonly IEntityRepository entityRepository;

        public EntitiesController(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var entities = await entityRepository.List();
            return View(entities);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(EntityViewModel model)
        {
            var entity = new LegalEntity
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

            var saved = entityRepository.Add(entity);
            await entityRepository.Save();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var entity = await entityRepository.Find(id);
            var model = new EntityViewModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                CompanyName = entity.CompanyName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                BankAccountNumber = entity.BankAccountNumber,
                Note = entity.Note,
                IdentifyingNumber = entity.IdentifyingNumber,
                TaxIdentifyingNumber = entity.TaxIdentifyingNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(long id, EntityViewModel model)
        {
            var entity = await entityRepository.Find(id);

            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.BankAccountNumber = model.BankAccountNumber;
            entity.Note = model.Note;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.CompanyName = model.CompanyName;
            entity.IdentifyingNumber = model.IdentifyingNumber;
            entity.TaxIdentifyingNumber = model.TaxIdentifyingNumber;

            await entityRepository.Save();

            return RedirectToAction(nameof(Detail), new { id = id });
        }

        [HttpGet]
        public ActionResult Detail(long id)
        {
            var entity = entityRepository.Find(id);
            return View(entity);
        }
    }

    public class EntityViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        /// <summary>
        /// ICO
        /// </summary>
        public string IdentifyingNumber { get; set; }

        /// <summary>
        /// DIC
        /// </summary>
        public string TaxIdentifyingNumber { get; set; }
    }
}