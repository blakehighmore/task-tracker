namespace TaskTracker.Services.Auth;

public interface IJwtTokenService
{
    string GenerateToken(Models.User user);
}