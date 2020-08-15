using Autofac;
using Lendee.Core.DataAccess;
using Lendee.Core.Domain.Interfaces;
using Lendee.Core.Domain.Payments;

namespace Lendee.Web.Configurations
{
    public class LendeeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<EntityRepository>().As<IEntityRepository>();
            //builder.RegisterType<ContractRepository>().As<IContractRepository>();
            //builder.RegisterType<RepaymentRepository>().As<IRepaymentRepository>();
            builder.RegisterType<PaymentFactory>();
            builder.RegisterType<MonthlyPaymentFactory>();

            builder.RegisterAssemblyTypes(typeof(ContractRepository).Assembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces();
            
                
        }
    }
}