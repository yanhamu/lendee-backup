using Lendee.Account.Domain;
using Lendee.Web.Features.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace Lendee.Web.Configurations.ServiceCollectionExtensions
{
    public static class UserServiceCollectionExtensions
    {
        public static void RegisterUserDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Service>();

            services.AddScoped<IDbConnection>(x => new SqlConnection(configuration.GetConnectionString("lendee")));
            services.AddTransient<IUserService, UserService>();
        }
    }
}