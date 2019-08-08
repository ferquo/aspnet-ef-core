using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkPlayground.Domain.Entitities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
