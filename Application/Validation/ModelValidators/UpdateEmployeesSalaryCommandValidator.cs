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
                .WithMessage("Id must be greater than zero");
            
            RuleFor(cmd => cmd.Salary)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Current salary must be non-negative");
        }
    }
}