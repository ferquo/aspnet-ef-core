using System.Collections.Generic;

namespace EntityFrameworkPlayground.Domain.DataTransferObjects
{
    public class LinkedCollectionResourceWrapperDTO<T> : LinkedResourceBaseDTO
        where T : LinkedResourceBaseDTO
    {
        public IEnumerable<T> Value { get; set; }

        public LinkedCollectionResourceWrapperDTO(IEnumerable<T> value)
        {
            Value = value;
        }
    }
}
