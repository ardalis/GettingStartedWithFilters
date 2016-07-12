using System.Collections.Generic;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Filters101.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Filters101.Controllers
{
    [Route("api/[controller]")]
    public class AuthersController : Controller
    {
        private readonly AppDbContext _db;

        public AuthersController(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<List<Author>> Get()
        {
            return await _db.Authors.ToListAsync(); 
        }
    }
}