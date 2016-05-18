using System.Collections.Generic;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Filters101.Interfaces;

namespace Filters101.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/authors
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            return _authorRepository.List();
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_authorRepository.List().All(a => a.Id != id))
            {
                return NotFound(id);
            }
            return Ok(_authorRepository.GetById(id));
        }

        // POST api/authors
        [HttpPost]
        public IActionResult Post([FromBody]Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _authorRepository.Add(author);
            return Ok(author);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Author author)
        {
            if (_authorRepository.List().All(a => a.Id != id))
            {
                return NotFound(id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            author.Id = id;
            _authorRepository.Update(author);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_authorRepository.List().All(a => a.Id != id))
            {
                return NotFound(id);
            }
            _authorRepository.Delete(id);
            return Ok();
        }

        // GET: api/authors/populate
        [HttpGet("Populate")]
        public IActionResult Populate()
        {
            if (!_authorRepository.List().Any())
            {
                _authorRepository.Add(new Author()
                {
                    Id = 1,
                    FullName = "Steve Smith",
                    TwitterAlias = "ardalis"
                });
                _authorRepository.Add(new Author()
                {
                    Id = 2,
                    FullName = "Neil Gaiman",
                    TwitterAlias = "neilhimself"
                });
            }
            return Ok();
        }
    }
}