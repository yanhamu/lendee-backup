using Lendee.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Repayment
{
    public class RepaymentFactory
    {
        private readonly IContractRepository contractRepository;
        private readonly IRepaymentRepository paymentRepository;

        public RepaymentFactory(IContractRepository contractRepository, IRepaymentRepository paymentRepository)
        {
            this.contractRepository = contractRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<ActualRepayment>> Calculate(long contractId)
        {
            var contract = await contractRepository.Find(contractId);
            var payments = await paymentRepository.List(contractId);
            IEnumerable<Repayment> repayments = GetRepayments(contract);
            return new RepaymentMatcher().Match(payments, repayments);
        }

        private static IEnumerable<Repayment> GetRepayments(Model.Contract contract)
        {
            switch (contract.PaymentTermType)
            {
                case Model.PaymentTermType.NotSet:
                    throw new NotImplementedException();
                case Model.PaymentTermType.Monthly:
                    return new MonthlyRepaymentCalculator().Build(new Input() { Amount = contract.PaymentAmount.Value, PaymentDay = contract.PaymentTermData.Day.Value, From = contract.ValidFrom });
                case Model.PaymentTermType.Quaterly:
                    break;
                case Model.PaymentTermType.HalfYear:
                    break;
                case Model.PaymentTermType.Annual:
                    break;
                case Model.PaymentTermType.Custom:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
