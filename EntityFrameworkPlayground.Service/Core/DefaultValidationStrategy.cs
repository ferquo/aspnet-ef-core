using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.Service.Core
{
    public class DefaultValidationStrategy : IValidationStrategy
    {
        public bool IsValid<T>(T validateThis) where T : class
        {
            return GetValidationResults(validateThis).Count == 0;
        }

        public IList<ValidationResult> GetValidationResults<T>(T model) where T : class
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            Validator.TryValidateObject(model, context, results, true);

            return results;
        }
    }
}
