using Lendee.Core.Domain.Model;
using System;
using System.Collections.Generic;

namespace Lendee.Core.Domain.Payments
{
    public class MonthlyPaymentFactory
    {
        public IEnumerable<Payment> BuildPayments(long contractId, PaymentSettings paymentSetting)
        {
            var date = paymentSetting.ValidFrom;
            while (date <= (paymentSetting.ValidUntil ?? DateTime.UtcNow))
            {
                var dueDate = new DateTime(date.Year, date.Month, paymentSetting.Day);
                yield return new Payment() { Amount = paymentSetting.PaymentAmount, DueDate = dueDate, ContractId = contractId };
                date = date.AddMonths(1);
            }
        }
    }
}