using System.Collections.Generic;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<List<Author>> Get()
        {
            return await _authorRepository.ListAsync();
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            return Ok(await _authorRepository.GetByIdAsync(id));
        }

        // POST api/authors
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authorRepository.AddAsync(author);
            return Ok(author);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Author author)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            author.Id = id;
            await _authorRepository.UpdateAsync(author);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _authorRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            await _authorRepository.DeleteAsync(id);
            return Ok();
        }

        // GET: api/authors/populate
        [HttpGet("Populate")]
        public async Task<IActionResult> Populate()
        {
            if (!(await _authorRepository.ListAsync()).Any())
            {
                await _authorRepository.AddAsync(new Author()
                {
                    Id = 1,
                    FullName = "Steve Smith",
                    TwitterAlias = "ardalis"
                });
                await _authorRepository.AddAsync(new Author()
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