using System;

namespace Lendee.Core.Domain.Model
{
    public class Repayment
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
