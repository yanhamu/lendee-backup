using System;

namespace Lendee.Core.Domain.Model
{
    public class Contract
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ContractType Type { get; set; }
        public Currency Currency { get; set; }
    }

    public enum ContractType : int
    {

        Credit = 1,
        Loan = 2,
        Rent = 3,
        // TODO postoupeni
    }

    public enum Currency
    {
        CZK = 1,
        EUR = 2,
        USC = 3
    }
}
