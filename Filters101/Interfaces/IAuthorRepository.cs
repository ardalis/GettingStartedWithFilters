using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filters101.Models;

namespace Filters101.Interfaces
{
    public interface IAuthorRepository
    {
        Author GetById(int id);
        IEnumerable<Author> List();
        void Update(Author author);
        void Add(Author author);
        void Delete(int id);
    }
}
