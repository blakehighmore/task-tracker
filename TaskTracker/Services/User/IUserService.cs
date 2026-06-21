using TaskTracker.DTOs.User;


namespace TaskTracker.Services.User;

public interface IUserService
{
    public Task<UserReadDto> RegisterAsync(UserRegisterDto dto);
    public Task<bool> LoginAsync(UserLoginDto dto);
}