using FluentValidation;
using TaskTracker.DTOs.Label;


namespace TaskTracker.Validators;

public class LabelCreateDtoValidator : AbstractValidator<LabelCreateDto>
{
    public LabelCreateDtoValidator()
    {
        RuleFor(l => l.Name)
            .NotEmpty().WithMessage("Name - обязательное поле")
            .MinimumLength(2).WithMessage("Минимальная длина названия - 2 символа")
            .MaximumLength(20).WithMessage("Максимальная длина названия - 20 символов");

        RuleFor(l => l.Color)
            .MinimumLength(2).WithMessage("Минимальная длина названия - 2 символа")
            .MaximumLength(20).WithMessage("Максимальная длина названия - 20 символов")
            .When(l => l.Color != null);
    }
}