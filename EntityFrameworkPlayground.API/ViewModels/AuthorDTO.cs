using System.Collections.Generic;

namespace EntityFrameworkPlayground.API.ViewModels
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookDTO> Books { get; set; }
    }
}
