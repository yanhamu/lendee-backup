namespace Lendee.Core.Domain.Model
{
    public class LegalEntity 
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string Note { get; set; }

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