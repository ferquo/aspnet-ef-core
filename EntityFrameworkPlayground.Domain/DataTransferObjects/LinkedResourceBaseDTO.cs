using System.Collections.Generic;

namespace EntityFrameworkPlayground.Domain.DataTransferObjects
{
    public abstract class LinkedResourceBaseDTO
    {
        public List<LinkDTO> Links { get; set; }
    }
}
