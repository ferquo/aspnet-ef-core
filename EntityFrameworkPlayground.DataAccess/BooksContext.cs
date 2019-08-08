using Microsoft.EntityFrameworkCore;
using EntityFrameworkPlayground.Domain.Entitities;

namespace EntityFrameworkPlayground.DataAccess
{
    public class BooksContext: DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> optionsBuilder)
            : base(optionsBuilder)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
