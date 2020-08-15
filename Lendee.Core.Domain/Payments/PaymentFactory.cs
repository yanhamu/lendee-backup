using Lendee.Core.Domain.Model;
using System;
using System.Collections.Generic;

namespace Lendee.Core.Domain.Payments
{
    public class PaymentFactory
    {
        private readonly MonthlyPaymentFactory monthlyPaymentFactory;

        public PaymentFactory(MonthlyPaymentFactory monthlyPaymentFactory)
        {
            this.monthlyPaymentFactory = monthlyPaymentFactory;
        }

        public IEnumerable<Payment> BuildPayments(long contractId, PaymentSettings paymentSetting)
        {
            switch (paymentSetting.PaymentTermType)
            {
                case PaymentTermType.NotSet:
                    break;
                case PaymentTermType.Monthly:
                    return monthlyPaymentFactory.BuildPayments(contractId, paymentSetting);
                case PaymentTermType.Quaterly:
                    break;
                case PaymentTermType.HalfYear:
                    break;
                case PaymentTermType.Annual:
                    break;
                case PaymentTermType.Custom:
                    break;
                default:
                    break;
            }
            throw new Exception("Payment term not supported");
        }
    }
}
