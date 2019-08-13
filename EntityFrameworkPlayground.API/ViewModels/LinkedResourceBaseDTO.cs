using System.Collections.Generic;

namespace EntityFrameworkPlayground.API.ViewModels
{
    public abstract class LinkedResourceBaseDTO
    {
        public List<LinkDTO> Links { get; set; }
    }
}
