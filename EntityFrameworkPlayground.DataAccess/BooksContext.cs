using Microsoft.EntityFrameworkCore;
using EntityFrameworkPlayground.Domain.Entitities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EntityFrameworkPlayground.DataAccess
{
    public class BooksContext: IdentityDbContext
    {
        public BooksContext(DbContextOptions<BooksContext> optionsBuilder)
            : base(optionsBuilder)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
