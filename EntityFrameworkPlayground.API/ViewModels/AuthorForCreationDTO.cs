using System.Collections.Generic;

namespace EntityFrameworkPlayground.API.ViewModels
{
    public class AuthorForCreationDTO
    {
        public string Name { get; set; }

        public ICollection<BookForCreationDTO> Books { get; set; }
    }
}
