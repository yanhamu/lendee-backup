using System;

namespace Lendee.Core.Domain.Model
{
    public class PaymentDue
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// Latest Payment Date (exclusive)
        /// </summary>
        public DateTime Due { get; set; }
    }
}
