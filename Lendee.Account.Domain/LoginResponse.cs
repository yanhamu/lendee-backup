using System;

namespace Lendee.Account.Domain
{
    public abstract class LoginResponse
    {
        public LoginResponse(bool isSuccessful)
        {
            this.IsSuccessful = isSuccessful;
        }
        public bool IsSuccessful { get; }
        public abstract Guid GetUserId();

        public static FailedLoginResponse CreateFailed()
        {
            return new FailedLoginResponse();
        }

        public static SuccessfulLoginResponse CreateSuccessful(Guid userId)
        {
            return new SuccessfulLoginResponse(userId);
        }
    }

    public class FailedLoginResponse : LoginResponse
    {
        public FailedLoginResponse() : base(false) { }

        public override Guid GetUserId()
        {
            throw new InvalidOperationException("Can't access unauthorized user id");
        }
    }

    public class SuccessfulLoginResponse : LoginResponse
    {
        private readonly Guid userId;

        public SuccessfulLoginResponse(Guid userId) : base(true)
        {
            this.userId = userId;
        }

        public override Guid GetUserId()
        {
            return userId;
        }
    }
}
