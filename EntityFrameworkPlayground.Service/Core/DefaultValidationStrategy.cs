using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.Service.Core
{
    public class DefaultValidationStrategy<T> : IValidationStrategy<T> where T : class
    {
        public bool IsValid(T validateThis)
        {
            return GetValidationResults(validateThis).Count == 0;
        }

        public IList<ValidationResult> GetValidationResults(T model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            Validator.TryValidateObject(model, context, results, true);

            return results;
        }
    }
}
