using System;
using System.Threading.Tasks;

namespace Lendee.Account.Domain
{
    public interface IUserService
    {
        Task<LoginResponse> Check(string username, string password);
        Task<bool> CheckAvailability(string username);
        Task<Guid> RegisterUser(string username, string password);
    }
}
