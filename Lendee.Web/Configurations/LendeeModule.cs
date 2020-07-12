using Autofac;
using Lendee.Core.DataAccess;
using Lendee.Core.Domain.Interfaces;

namespace Lendee.Web.Configurations
{
    public class LendeeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityRepository>().As<IEntityRepository>();
            builder.RegisterType<ContractRepository>().As<IContractRepository>();
        }
    }
}