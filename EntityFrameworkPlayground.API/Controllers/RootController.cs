using EntityFrameworkPlayground.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult Get([FromHeader(Name = "Accept")]string mediaType)
        {
            if (mediaType == "application/vnd.hateoas+json")
            {
                var links = new List<LinkDTO>();
                links.Add(new LinkDTO(
                    href: urlHelper.Link("GetRoot", new { }),
                    rel: "self",
                    method: "GET"
                    ));
                links.Add(new LinkDTO(
                    href: urlHelper.Link("GetAuthors", new { }),
                    rel: "authors",
                    method: "GET"
                    ));
                links.Add(new LinkDTO(
                    href: urlHelper.Link("CreateAuthor", new { }),
                    rel: "create-author",
                    method: "POST"
                    ));
                return Ok(links);
            }
            else
            {
                return NoContent();
            }
        }
    }
}