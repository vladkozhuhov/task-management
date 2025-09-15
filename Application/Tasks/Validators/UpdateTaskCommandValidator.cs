using Application.Tasks.Commands;
using FluentValidation;

namespace Application.Tasks.Validators;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id задачи обязателен");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Заголовок задачи не должен превышать 200 символов")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание задачи не должно превышать 2000 символов")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || 
                           status == "New" || 
                           status == "InProgress" || 
                           status == "Done")
            .WithMessage("Статус должен быть New, InProgress или Done")
            .When(x => !string.IsNullOrEmpty(x.Status));

        RuleFor(x => x.Priority)
            .Must(priority => string.IsNullOrEmpty(priority) || 
                             priority == "Low" || 
                             priority == "Medium" || 
                             priority == "High")
            .WithMessage("Приоритет должен быть Low, Medium или High")
            .When(x => !string.IsNullOrEmpty(x.Priority));
    }
}
