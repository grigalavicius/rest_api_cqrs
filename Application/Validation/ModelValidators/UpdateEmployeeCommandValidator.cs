using System;
using Application.Commands.UpdateEmployee;
using DataStore.Models;
using FluentValidation;

namespace Application.Validation.ModelValidators
{
    internal class UpdateEmployeeCommandValidator : Validator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.Id)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.IdMustBeGreaterThanZero);

            RuleFor(cmd => cmd.FirstName)
                .NotEmpty()
                .MaximumLength(50);
            
            RuleFor(cmd => cmd.LastName)
                .NotEmpty()
                .MaximumLength(50);


            RuleFor(cmd => cmd)
                .Must(cmd => cmd.FirstName != cmd.LastName)
                .WithMessage(ValidationMessages.NotEqualFirstAndLastNamesValidationMessage);
            
            RuleFor(cmd => cmd.BirthDate)
                .NotEmpty()
                .Must(birthDate => DateTime.Today >= birthDate.Date.AddYears(18))
                .WithMessage(ValidationMessages.EmployeeMustBeAtLeast18YearsOld)
                .Must(birthDate => DateTime.Today <= birthDate.Date.AddYears(70))
                .WithMessage(ValidationMessages.EmployeeMustBeNotOlderThan70Years);
            
            RuleFor(cmd => cmd.EmploymentDate)
                .NotEmpty()
                .GreaterThan(new DateTime(2000,01,01))
                .WithMessage(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate)
                .LessThanOrEqualTo(DateTime.Today.AddDays(1).AddMinutes(-1))
                .WithMessage(ValidationMessages.EmploymentDateCannotBeEarlierThanAndCannotBeFutureDate);
            
            RuleFor(cmd => cmd.HomeAddress)
                .NotEmpty();
            
            RuleFor(cmd => cmd.Salary)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(ValidationMessages.CurrentSalaryMustBeNonNegative);

            RuleFor(cmd => cmd.Role)
                .NotNull();

            When(cmd => cmd.Role == Role.Ceo, () =>
                {
                    RuleFor(cmd => cmd.BossId)
                        .Null()
                        .WithMessage(ValidationMessages.BossIdMustBeNull);
                })
                .Otherwise(() =>
                {
                    RuleFor(cmd => cmd.BossId)
                        .NotNull()
                        .WithMessage(ValidationMessages.BossIdMustBeNotNull);
                });
        }
    }
}