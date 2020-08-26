namespace Lendee.Core.Domain.Model
{
    public class Loan : Contract
    {
        public decimal Amount { get; set; }
    }

    public class LoanWithInterest : Contract
    {
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
    }
}