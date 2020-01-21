using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
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

        public PagedList<Author> GetAllAuthors(PagingResourceParameters paging)
        {
            var query = (IQueryable<Author>)db.Authors
                .Include(x => x.Books)
                .OrderBy(author => author.Name);

            if (!string.IsNullOrEmpty(paging.SearchQuery))
            {
                query = query.Where(x => x.Name.ToLowerInvariant().Contains(paging.SearchQuery.ToLowerInvariant()));
            }

            return PagedList<Author>.Create(query, paging.PageNumber, paging.PageSize);
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await db.Authors.Include(x => x.Books).SingleOrDefaultAsync(x => x.AuthorId == id);
        }

        public async Task<bool> Exists(int id)
            => await db.Authors.AsNoTracking().AnyAsync(x => x.AuthorId == id);
    }
}
