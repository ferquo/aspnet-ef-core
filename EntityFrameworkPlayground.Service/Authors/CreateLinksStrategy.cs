using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityFrameworkPlayground.Service.Authors
{
    public class CreateLinksStrategy : ICreateLinksStrategy
    {
        private readonly IUrlHelper urlHelper;

        public CreateLinksStrategy(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public LinkedCollectionResourceWrapperDTO<AuthorDTO> CreateLinksForAuthors(
            LinkedCollectionResourceWrapperDTO<AuthorDTO> authorsWrapper,
            PagingResourceParameters pagingResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            authorsWrapper.Links = new List<LinkDTO>();

            authorsWrapper.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthors", new { }),
                rel: "self",
                method: "GET"));

            if (hasPrevious)
            {
                authorsWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetAuthors", new
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
                authorsWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetAuthors", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber + 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "next",
                    method: "GET"));
            }

            return authorsWrapper;
        }

        public AuthorDTO CreateLinksForAuthorResource(AuthorDTO author)
        {
            author.Links = new List<LinkDTO>();

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthor", new { id = author.Id }),
                rel: "self",
                method: "GET"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBooks", new { authorId = author.Id }),
                rel: "children",
                method: "GET"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("CreateAuthor", null),
                rel: "create-author",
                method: "POST"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateAuthor", new { id = author.Id }),
                rel: "update-author",
                method: "PUT"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("DeleteAuthor", new { id = author.Id }),
                rel: "delete-author",
                method: "DELETE"));

            return author;
        }
    }
}
