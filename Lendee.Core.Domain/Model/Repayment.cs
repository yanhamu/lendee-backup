using System;

namespace Lendee.Core.Domain.Model
{
    /// <summary>
    /// Splatka
    /// </summary>
    public abstract class Repayment
    {
        public long Id { get; set; }
        public DateTime DueDate { get; set; }
        public long ContractId { get; set; }
        public Contract Contract { get; set; }
    }

    public class RentRepayment : Repayment
    {
        public decimal Amount { get; set; }
    }

    public class CombinedRentRepayment : Repayment
    {
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
    }

    public class VariableRentRepayment : Repayment
    {
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class LoanRepayment : Repayment
    {
        public decimal Amount { get; set; }
    }
}
