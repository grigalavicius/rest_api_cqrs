using Application.Commands.UpdateEmployeesSalary;
using FluentValidation;

namespace Application.Validation.ModelValidators
{
    internal class UpdateEmployeesSalaryCommandValidator: Validator<UpdateEmployeesSalaryCommand>
    {
        public UpdateEmployeesSalaryCommandValidator()
        {
            RuleFor(cmd => cmd.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.IdMustBeGreaterThanZero);
            
            RuleFor(cmd => cmd.Salary)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(ValidationMessages.CurrentSalaryMustBeNonNegative);
        }
    }
}