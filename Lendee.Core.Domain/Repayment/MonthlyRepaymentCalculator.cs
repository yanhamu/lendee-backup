using System;
using System.Collections.Generic;

namespace Lendee.Core.Domain.Repayment
{
    public class MonthlyRepaymentCalculator
    {
        public IEnumerable<Repayment> Build(Input input)
        {
            var date = input.From.Date;
            var payments = new List<Repayment>();
            while (date < DateTime.Now.Date)
            {
                var startDate = new DateTime(date.Year, date.Month, date.Day);
                var tempDate = date.AddMonths(1);
                var endDate = new DateTime(tempDate.Year, tempDate.Month, input.PaymentDay);
                yield return new Repayment(new Interval(startDate, endDate), input.Amount);
                date = endDate.AddDays(1);
            }
        }
    }

    public class Input
    {
        public DateTime From { get; set; }
        public int PaymentDay { get; set; }
        public decimal Amount { get; set; }
    }

    public class Repayment
    {
        public Repayment(Interval interval, decimal toPay)
        {
            Interval = interval;
            ToPay = toPay;
        }

        public Interval Interval { get; set; }
        public decimal ToPay { get; set; }
    }

    public struct Interval
    {
        public Interval(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateTime From { get; }
        public DateTime To { get; }
    }
}
