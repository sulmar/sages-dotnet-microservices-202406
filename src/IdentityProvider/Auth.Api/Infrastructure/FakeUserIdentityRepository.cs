using Auth.Api.Abstractions;
using Auth.Api.Model;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class FakeUserIdentityRepository(IPasswordHasher<UserIdentity> _passwordHasher) : IUserIdentityRepository
{
    public Task<UserIdentity> GetAsync(string username)
    {
        var identity = new UserIdentity
        {
            Username = "marcin",
            FirstName = "Marcin",
            LastName = "Sulecki",
            Email = "marcin.sulecki@sulmar.pl"
        };

        // TODO: zahashuj hasło!
        identity.HashedPassword = _passwordHasher.HashPassword(identity, "123");

        return Task.FromResult(identity);
    }
}

public class FakeTokenService : ITokenService
{
    public string CreateAccessToken(UserIdentity identity)
    {
        return "your-jwt-token";
    }
}