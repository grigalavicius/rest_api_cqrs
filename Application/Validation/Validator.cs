using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Validation
{
    internal abstract class Validator<T> : FluentValidation.AbstractValidator<T>
    {
        public void ValidateAndThrow(T instance)
        {
            try
            {
                FluentValidation.DefaultValidatorExtensions.ValidateAndThrow(this, instance);
            }
            catch (FluentValidation.ValidationException e)
            {
                throw new ValidationException(e.Message, e);
            }
        }

        public async Task ValidateAndThrowAsync(T instance)
        {
            try
            {
                await FluentValidation.DefaultValidatorExtensions.ValidateAndThrowAsync(this, instance);
            }
            catch (FluentValidation.ValidationException e)
            {
                throw new ValidationException(e.Message, e);
            }
        }
    }
}