using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IUpdateBookStrategy
    {
        Task<BookForUpdateDTO> ApplyPatch(int bookId, JsonPatchDocument<BookForUpdateDTO> patchDoc);
        Task<bool> AuthorExists(int authorId);
        Task<bool> BookExists(int bookId);
        Task<BookDTO> UpdateBook(int bookId, BookForUpdateDTO value);
    }
}