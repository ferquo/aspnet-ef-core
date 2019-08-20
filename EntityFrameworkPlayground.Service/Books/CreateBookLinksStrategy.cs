using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityFrameworkPlayground.Service.Books
{
    public class CreateBookLinksStrategy : ICreateBookLinksStrategy
    {
        private readonly IUrlHelper urlHelper;

        public CreateBookLinksStrategy(
            IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public BookDTO CreateLinksForBookResource(BookDTO book)
        {
            book.Links = new List<LinkDTO>();

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "self",
                method: "GET"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthor", new { id = book.AuthorId }),
                rel: "parrent",
                method: "GET"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("CreateBook", new { authorId = book.AuthorId }),
                rel: "create-book",
                method: "POST"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "update-book",
                method: "PUT"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "update-partial-book",
                method: "PATCH"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("DeleteBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "delete-book",
                method: "DELETE"));

            return book;
        }

        public LinkedCollectionResourceWrapperDTO<BookDTO> CreateLinksForBooks(
            LinkedCollectionResourceWrapperDTO<BookDTO> booksWrapper,
            PagingResourceParameters pagingResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            booksWrapper.Links = new List<LinkDTO>();

            booksWrapper.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBooks", new { }),
                rel: "self",
                method: "GET"));

            if (hasPrevious)
            {
                booksWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetBooks", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber - 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "previous",
                    method: "GET"));
            }

            if (hasNext)
            {
                booksWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetBooks", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber + 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "next",
                    method: "GET"));
            }

            return booksWrapper;
        }
    }
}
