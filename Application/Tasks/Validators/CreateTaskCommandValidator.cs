using Application.Tasks.Commands;
using FluentValidation;

namespace Application.Tasks.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок задачи обязателен")
            .MaximumLength(200).WithMessage("Заголовок задачи не должен превышать 200 символов");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание задачи не должно превышать 2000 символов");

        RuleFor(x => x.Priority)
            .Must(priority => string.IsNullOrEmpty(priority) || 
                             priority == "Low" || 
                             priority == "Medium" || 
                             priority == "High")
            .WithMessage("Приоритет должен быть Low, Medium или High");
    }
}
