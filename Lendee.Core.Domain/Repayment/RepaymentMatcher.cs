using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Lendee.Core.Domain.Repayment
{
    public class RepaymentMatcher
    {
        public IEnumerable<ActualRepayment> Match(IEnumerable<Payment> payments, IEnumerable<Repayment> repayments)
        {
            var repaymentsEnumerator = repayments.GetEnumerator();
            var paymentsEnumerator = payments.OrderBy(x => x.PaidAt).GetEnumerator();
            foreach (var repayment in repayments)
            {
                var p = payments.Where(x => x.PaidAt <= repayment.Interval.To && x.PaidAt >= repayment.Interval.From).ToArray();
                yield return new ActualRepayment(repayment, p);
            }
        }
    }

    public class ActualRepayment : Repayment
    {
        public decimal Paid { get => Payments.Sum(p => p.Amount); }
        public Payment[] Payments { get; set; }
        public ActualRepayment(Repayment repayment, Payment[] payments) : base(repayment.Interval, repayment.ToPay)
        {
            this.ToPay = repayment.ToPay;
            this.Interval = repayment.Interval;
            this.Payments = payments;
        }
    }
}