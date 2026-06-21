using Microsoft.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.DTOs.User;
using TaskTracker.Exceptions;


namespace TaskTracker.Services.User;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserReadDto> RegisterAsync(UserRegisterDto dto)
    {
        var taken = await _db.Users.AnyAsync(u => u.Username == dto.Username);

        if (taken) throw new ConflictException("Такой username уже существует");

        var user = new Models.User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new UserReadDto(user.Id, user.Username);
    }

    public async Task<bool> LoginAsync(UserLoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user is null) throw new NotFoundException("Такого пользователя не существует");

        return BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
    }
}