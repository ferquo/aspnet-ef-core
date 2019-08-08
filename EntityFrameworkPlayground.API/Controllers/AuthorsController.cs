﻿using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthorRepository authorRepository;

        public AuthorsController(
            IMapper mapper,
            IAuthorRepository authorRepository)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
        }

        // GET: api/authors
        [HttpGet]
        public IActionResult Get()
        {
            var authors = authorRepository.GetAllAuthors();
            return Ok(mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        // GET: api/Authors/5
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<IActionResult> Get(int id)
        {
            var author = await authorRepository.GetAuthorById(id);
            if (author == null)
            {
                NotFound();
            }
            return Ok(mapper.Map<AuthorDTO>(author));
        }

        // POST: api/Authors
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
