using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Lendee.Account.Domain
{
    public class UserService : IUserService
    {
        private readonly IDbConnection connection;

        public UserService(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<LoginResponse> Check(string username, string password)
        {
            UserDto userDto = await GetUser(username);
            if (userDto is null)
                return LoginResponse.CreateFailed();

            return new PasswordService().Verify(userDto.Password, password)
                ? (LoginResponse)LoginResponse.CreateSuccessful(userDto.Id)
                : LoginResponse.CreateFailed();
        }

        public async Task<bool> CheckAvailability(string username)
        {
            var user = await GetUser(username);
            return user is null;
        }

        public async Task<Guid> RegisterUser(string username, string password)
        {
            var s = new PasswordService();
            var hashedPassword = s.GetHashedPassword(password);
            var id = Guid.NewGuid();
            var sql = @"insert into lendee.users values (@id, @username, @password)";
            await connection.ExecuteAsync(sql, new { id, username, password = hashedPassword });
            return id;
        }

        private async Task<UserDto> GetUser(string username)
        {
            var sql = "select id, password from lendee.users where username = @username";
            var userDto = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { username });
            return userDto;
        }

        internal class UserDto
        {
            public Guid Id { get; set; }
            public string Password { get; set; }
        }
    }
}
