using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Model;

public record LoginModel(string Username, string Password);
