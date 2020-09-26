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
            builder.RegisterAssemblyTypes(typeof(ContractRepository).Assembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType<PaymentService>();
        }
    }
}