using System;
using Application.Commands.CreateEmployee;
using DataStore.Models;
using FluentValidation;

namespace Application.Validation.ModelValidators
{
    internal class CreateEmployeeCommandValidator : Validator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(cmd => cmd.FirstName)
                .NotEmpty()
                .MaximumLength(50);
            
            RuleFor(cmd => cmd.LastName)
                .NotEmpty()
                .MaximumLength(50);


            RuleFor(cmd => cmd)
                .Must(cmd => cmd.FirstName != cmd.LastName)
                .WithMessage("FirstName can not coincide with the LastName");
            
            RuleFor(cmd => cmd.BirthDate)
                .NotEmpty()
                .Must(birthDate => DateTime.Today >= birthDate.Date.AddYears(18))
                .WithMessage("Employee must be at least 18 years old")
                .Must(birthDate => DateTime.Today <= birthDate.Date.AddYears(70))
                .WithMessage("Employee must be not older than 70 years");
            
            RuleFor(cmd => cmd.EmploymentDate)
                .NotEmpty()
                .GreaterThan(new DateTime(2000,01,01))
                .LessThanOrEqualTo(DateTime.Today.AddDays(1).AddMinutes(-1))
                .WithMessage("Employment date cannot be earlier than 2000-01-01 and cannot be future date");
            
            RuleFor(cmd => cmd.HomeAddress)
                .NotEmpty();
            
            RuleFor(cmd => cmd.Salary)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Current salary must be non-negative");

            RuleFor(cmd => cmd.Role)
                .NotNull();

            When(cmd => cmd.Role == Role.Ceo, () =>
                {
                    RuleFor(cmd => cmd.BossId)
                        .Null()
                        .WithMessage("BossId must be null");
                })
                .Otherwise(() =>
                {
                    RuleFor(cmd => cmd.BossId)
                        .NotNull()
                        .WithMessage("BossId must be not null");
                });
        }
    }
}