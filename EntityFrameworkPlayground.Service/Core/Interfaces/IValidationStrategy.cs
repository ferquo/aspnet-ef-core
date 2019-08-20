using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.Service.Core
{
    public interface IValidationStrategy<T> where T : class
    {
        IList<ValidationResult> GetValidationResults(T model);
        bool IsValid(T validateThis);
    }
}