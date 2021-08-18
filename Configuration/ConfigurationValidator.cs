using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Configuration
{
    internal class ConfigurationValidator
    {
        internal static void TryValidateObject(object validationObject)
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(validationObject, new ValidationContext(validationObject), validationResults, true))
            {
                throw new ValidationException(validationResults.First().ErrorMessage);
            }
        }
    }
}