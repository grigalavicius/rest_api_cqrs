using Application.Commands.DeleteEmployee;
using FluentValidation;

namespace Application.Validation.ModelValidators
{
    internal class DeleteEmployeeCommandValidator : Validator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero");
        }
    }
}