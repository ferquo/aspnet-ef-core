using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkPlayground.Service.Core
{
    public interface IValidationStrategy
    {
        IList<ValidationResult> GetValidationResults<T>(T model) where T : class;
        bool IsValid<T>(T validateThis) where T : class;
    }
}