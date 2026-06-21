using FluentValidation;
using TaskTracker.DTOs.TaskItem;


namespace TaskTracker.Validators;

public class TaskItemUpdateDtoValidator : AbstractValidator<TaskItemUpdateDto>
{
    public TaskItemUpdateDtoValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty().WithMessage("Название обязательно")
            .MinimumLength(10).WithMessage("Минимальное количество символов - 10")
            .MaximumLength(200).WithMessage("Максимальное количество символов - 200");

        RuleFor(t => t.Description)
            .MinimumLength(10).WithMessage("Минимальное количество символов - 10")
            .MaximumLength(200).WithMessage("Максимальное количество символов - 200")
            .When(t => t.Description != null);
    }
}