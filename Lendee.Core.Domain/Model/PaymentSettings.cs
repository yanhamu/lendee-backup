using System;

namespace Lendee.Core.Domain.Model
{
    public class PaymentSettings
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public PaymentTermType PaymentTermType { get; set; }
        public int Day { get; set; }
        public int? Month { get; set; }
    }
}
