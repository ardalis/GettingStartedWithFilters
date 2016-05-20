using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Filters101.Models;

namespace Filters101.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(int id);
        Task<List<Author>> ListAsync();
        Task UpdateAsync(Author author);
        Task AddAsync(Author author);
        Task DeleteAsync(int id);
    }
}
