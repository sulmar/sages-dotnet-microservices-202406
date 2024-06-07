using Auth.Api.Abstractions;
using Auth.Api.Model;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class AuthService(
    IUserIdentityRepository _repository, 
    IPasswordHasher<UserIdentity> _passwordHasher) : IAuthService
{
    public async Task<AuthorizeResult> AuthorizeAsync(string username, string password)
    {
        var identity = await _repository.GetAsync(username);

        var result = _passwordHasher.VerifyHashedPassword(identity, identity.HashedPassword, password);

        if (result == PasswordVerificationResult.Success)
        {
            return new AuthorizeResult(true, identity);
        }
        else
            return new AuthorizeResult(false, identity);
    }
}
