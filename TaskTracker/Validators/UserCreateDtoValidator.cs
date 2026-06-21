using FluentValidation;
using TaskTracker.DTOs.User;


namespace TaskTracker.Validators;

public class UserCreateDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Username обязательное поле")
            .MinimumLength(3).WithMessage("Минимальное количество символов - 3")
            .MaximumLength(100).WithMessage("Максимальное количество символов - 200");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Пароль обязательное поле")
            .MinimumLength(3).WithMessage("Минимальное количество символов - 3")
            .MaximumLength(72).WithMessage("Максимальное количество символов - 72");
    }
}