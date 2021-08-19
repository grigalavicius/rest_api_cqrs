using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Validation
{
    internal abstract class Validator<T> : FluentValidation.AbstractValidator<T>
    {
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