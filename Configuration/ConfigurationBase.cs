using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Configuration
{
    public abstract class ConfigurationBase
    {
        public void TryValidateObject()
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true))
            {
                throw new ValidationException(validationResults.First().ErrorMessage);
            }
        }
    }
}