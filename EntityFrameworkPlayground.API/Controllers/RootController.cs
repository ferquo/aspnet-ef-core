using EntityFrameworkPlayground.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly IUrlHelper urlHelper;
        private readonly UserManager<IdentityUser> userManager;

        public RootController(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult Get([FromHeader(Name = "Accept")]string mediaType)
        {
            userManager.CreateAsync(new IdentityUser { Email = "f@f.com" }, "secure_password");

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