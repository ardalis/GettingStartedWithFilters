using System.Collections.Generic;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Filters101.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

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
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await _authorRepository.ListAsync();
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        //[ProducesResponseType(200, Type=typeof(Author))]
        public async Task<IActionResult> Get(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(id);
            }
            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public async Task<ActionResult<Author>> Post([FromBody]Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authorRepository.AddAsync(author);
            return Ok(author);
        }

        // PUT api/authors
        [HttpPut()]
        public async Task<ActionResult<Author>> Put([FromBody]Author author)
        {
            var authorToUpdate = await _authorRepository.GetByIdAsync(author.Id);
            if (authorToUpdate == null)
            {
                return NotFound(author.Id);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            authorToUpdate.FullName = author.FullName;
            authorToUpdate.TwitterAlias = author.TwitterAlias;
            await _authorRepository.UpdateAsync(authorToUpdate);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound(id);
            }
            await _authorRepository.DeleteAsync(id);
            return Ok();
        }

        // equivalent to a controller-specific action filter 👇
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            // do work
            await next();

            // do other work
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