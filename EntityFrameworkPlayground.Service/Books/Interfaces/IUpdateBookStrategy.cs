using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IUpdateBookStrategy
    {
        Task<BookForUpdateDTO> ApplyPatch(int authorId, int bookId, JsonPatchDocument<BookForUpdateDTO> patchDoc);
        Task<BookDTO> UpdateBook(int authorId, int bookId, BookForUpdateDTO value);
    }
}