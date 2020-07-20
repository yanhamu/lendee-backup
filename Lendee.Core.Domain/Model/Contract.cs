﻿using System;

namespace Lendee.Core.Domain.Model
{
    public class Contract
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ContractType Type { get; set; }
        public Currency Currency { get; set; }
        public long? LenderId { get; set; }
        public LegalEntity Lender { get; set; }
        public long? LendeeId { get; set; }
        public LegalEntity Lendee { get; set; }
        public string Note { get; set; }
        public PaymentTermType PaymentTermType { get; set; }
        public PaymentTerm PaymentTermData { get; set; }
        public decimal? PaymentAmount { get; set; }
    }

    public class PaymentTerm
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
    }

    public class Rent : Contract
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    public class Credit : Contract
    {
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PrincipalSum { get; set; }
    }

    public enum PaymentTermType
    {
        NotSet = 0,
        Monthly = 1,
        Quaterly = 2,
        HalfYear = 3,
        Annual = 4,
        Custom = 5
    }

    public enum ContractType : int
    {
        Draft = 0,
        Credit = 1, // uver
        Loan = 2, // pujcka
        Rent = 3, // najem
        // MISSING postoupeni
    }

    public enum Currency
    {
        CZK = 1,
        EUR = 2,
        USD = 3
    }
}
