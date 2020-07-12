using Autofac;
using Lendee.Web.Features.Common;

namespace Lendee.Web.Configurations
{
    public class LendeeWebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LegalEntityFactory>();
        }
    }
}