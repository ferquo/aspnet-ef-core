using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(BooksContext db) : base(db)
        {
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return db.Authors.Include(x => x.Books).ToList();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await db.Authors.Include(x => x.Books).SingleAsync(x => x.AuthorId == id);
        }

        public async Task<bool> Exists(int id)
            => await db.Authors.AsNoTracking().AnyAsync(x => x.AuthorId == id);
    }
}
