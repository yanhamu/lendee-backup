using Lendee.Core.Domain.Model;
using System;
using System.Collections.Generic;

namespace Lendee.Core.Domain.Repayments
{
    public class LoanRepaymentFactory
    {
        public IEnumerable<LoanRepayment> Generate(Loan loan)
        {
            var date = new DateTime(loan.ValidFrom.Year, loan.ValidFrom.Month, loan.PaymentTermData.Day.Value);
            var i = 0;
            var repayments = new List<LoanRepayment>();
            while (date <= loan.ValidUntil.Value)
            {
                repayments.Add(new LoanRepayment() { ContractId = loan.Id, DueDate = date });
                date = date.AddMonths(1);
                i += 1;
            }

            var repaymentAmount = loan.Amount / i;
            repayments.ForEach(r => r.Amount = repaymentAmount);
            return repayments;
        }
    }
}