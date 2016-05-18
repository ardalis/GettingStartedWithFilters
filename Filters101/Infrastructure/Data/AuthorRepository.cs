using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filters101.Interfaces;
using Filters101.Models;
using Microsoft.EntityFrameworkCore;

namespace Filters101.Infrastructure.Data
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Author GetById(int id)
        {
            return _dbContext.Authors.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Author> List()
        {
            return _dbContext.Authors;
        }

        public void Update(Author author)
        {
            _dbContext.Entry(author).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Add(Author author)
        {
            if (!_dbContext.Authors.Any())
            {
                author.Id = 1;
            }
            else
            {
                int maxId = _dbContext.Authors.Max(a => a.Id);
                author.Id = maxId + 1;
            }
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();
        }
    }
}
