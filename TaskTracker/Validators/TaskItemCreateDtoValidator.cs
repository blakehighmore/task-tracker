using FluentValidation;
using TaskTracker.DTOs.TaskItem;


namespace TaskTracker.Validators;

public class TaskItemCreateDtoValidator : AbstractValidator<TaskItemCreateDto>
{
    public TaskItemCreateDtoValidator()
    {
        RuleFor(t => t.ProjectId)
            .GreaterThan(0);

        RuleFor(t => t.Status)
            .IsInEnum();

        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Название обязательно")
            .MinimumLength(3).WithMessage("Минимальное количество символов - 10")
            .MaximumLength(200).WithMessage("Максимальное количество символов - 200");

        RuleFor(t => t.Description)
            .MinimumLength(10).WithMessage("Минимальное количество символов - 10")
            .MaximumLength(200).WithMessage("Максимальное количество символов - 200")
            .When(t => t.Description != null);
    }
}