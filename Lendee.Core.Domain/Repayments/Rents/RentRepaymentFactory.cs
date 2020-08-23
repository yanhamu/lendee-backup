using Lendee.Core.Domain.Model;
using System;
using System.Collections.Generic;

namespace Lendee.Core.Domain.Repayments.Rents
{
    public abstract class RepaymentFactory<TIn, TOut> where TIn : Contract
    {
        public IEnumerable<TOut> Generate(TIn contract)
        {
            var untilDate = GetUntilDate(contract);

            var date = new DateTime(contract.ValidFrom.Year, contract.ValidFrom.Month, contract.PaymentTermData.Day.Value);
            while (date <= untilDate)
            {
                yield return CreateRepayment(contract, date);
                date = date.AddMonths(1);
            }
        }

        protected abstract TOut CreateRepayment(TIn rent, DateTime date);

        private DateTime GetUntilDate(Contract rent)
        {
            if (rent.ValidUntil.HasValue)
                return rent.ValidUntil.Value;

            return rent.PaymentTermData.Day.Value <= DateTime.Now.Day
                ? new DateTime(DateTime.Now.Year, DateTime.Now.Year, rent.PaymentTermData.Day.Value).AddMonths(1)
                : new DateTime(DateTime.Now.Year, DateTime.Now.Year, rent.PaymentTermData.Day.Value);
        }
    }

    public class RentRepaymentFactory : RepaymentFactory<Rent, RentRepayment>
    {
        protected override RentRepayment CreateRepayment(Rent rent, DateTime date) => new RentRepayment() { Amount = rent.Amount, ContractId = rent.Id, DueDate = date };
    }

    public class CombinedRentRepaymentFactory : RepaymentFactory<CombinedRent, CombinedRentRepayment>
    {
        protected override CombinedRentRepayment CreateRepayment(CombinedRent rent, DateTime date) => new CombinedRentRepayment() { Amount = rent.Amount, Fee = rent.Fee, ContractId = rent.Id, DueDate = date };
    }

    public class VariableRentRepaymentFactory : RepaymentFactory<VariableRent, VariableRentRepayment>
    {
        protected override VariableRentRepayment CreateRepayment(VariableRent rent, DateTime date) => new VariableRentRepayment() { Amount = 1, UnitPrice = rent.UnitPrice, ContractId = rent.Id, DueDate = date };
    }
}
