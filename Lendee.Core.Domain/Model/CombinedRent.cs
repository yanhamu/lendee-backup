namespace Lendee.Core.Domain.Model
{
    public class CombinedRent : Contract
    {
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
    }

    public class VariableRent : Contract
    {
        public decimal UnitPrice { get; set; }
    }

    public class Rent : Contract
    {
        public decimal Amount { get; set; }
    }
}