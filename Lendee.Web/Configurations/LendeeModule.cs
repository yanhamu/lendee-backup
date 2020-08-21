using Autofac;
using Lendee.Core.DataAccess;

namespace Lendee.Web.Configurations
{
    public class LendeeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ContractRepository).Assembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
    }
}