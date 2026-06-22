using FluentValidation;
using TaskTracker.DTOs.Project;


namespace TaskTracker.Validators;

public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
{
    public ProjectCreateDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Название проекта обязательно")
            .MinimumLength(3).WithMessage("Минимальная длина названия 3 символа")
            .MaximumLength(200).WithMessage("Максимальная длина названия 200 символов");

        RuleFor(p => p.Description)
            .MinimumLength(3).WithMessage("Минимальная длина описания 3 символа")
            .MaximumLength(200).WithMessage("Максимальная длина описания 200 символов")
            .When(t => t.Description != null);
    }
}