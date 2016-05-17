using System.Collections.Generic;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using Filters101.Filters;
using Filters101.Interfaces;

namespace Filters101.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class Authors2Controller : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public Authors2Controller(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/authors2
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            return _authorRepository.List();
        }

        // GET api/authors2/5
        [HttpGet("{id}")]
        [ValidateAuthorExists]
        public IActionResult Get(int id)
        {
            return Ok(_authorRepository.GetById(id));
        }

        // POST api/authors2
        [HttpPost]
        public IActionResult Post([FromBody]Author author)
        {
            _authorRepository.Add(author);
            return Ok();
        }

        // PUT api/authors2/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Author author)
        {
            _authorRepository.Update(author);
            return Ok();
        }

        // DELETE api/authors2/5
        [HttpDelete("{id}")]
        [ValidateAuthorExists]
        public IActionResult Delete(int id)
        {
            _authorRepository.Delete(id);
            return Ok();
        }
    }
}