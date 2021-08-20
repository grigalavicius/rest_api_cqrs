using Application.Queries.GetEmployeesByNameAndBirthdateInterval;
using FluentValidation;

namespace Application.Validation.ModelValidators
{
    internal class GetEmployeesByNameAndBirthdateIntervalQueryValidator : Validator<GetEmployeesByNameAndBirthdateIntervalQuery>
    {
        public GetEmployeesByNameAndBirthdateIntervalQueryValidator()
        {
            RuleFor(query => query.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(query => query)
                .Must(query => query.From <= query.To)
                .WithMessage(ValidationMessages.DateFromMustBeLessOrEqualToDateTo);
        }
    }
}