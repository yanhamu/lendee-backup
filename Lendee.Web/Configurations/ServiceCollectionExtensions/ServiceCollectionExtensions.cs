using Lendee.Account.Domain;
using Lendee.Core.DataAccess;
using Lendee.Core.Domain.Repayment;
using Lendee.Web.Features.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace Lendee.Web.Configurations.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterUserDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Service>();

            services.AddScoped<IDbConnection>(x => new SqlConnection(configuration.GetConnectionString("lendee")));
            services.AddDbContext<LendeeContext>(options => { options.UseSqlServer(configuration.GetConnectionString("lendee")); });
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<RepaymentFactory>();
        }
    }
}