using System.Threading.Tasks;
using Filters101.Interfaces;
using Filters101.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters101.Filters
{
    public class ValidateAuthorExistsAttribute : TypeFilterAttribute
    {
        public ValidateAuthorExistsAttribute() : base(typeof(ValidateAuthorExistsFilterImpl))
        {
        }

        private class ValidateAuthorExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IAuthorRepository _authorRepository;

            public ValidateAuthorExistsFilterImpl(IAuthorRepository authorRepository)
            {
                _authorRepository = authorRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                int? authorId = null;
                if (context.ActionArguments.ContainsKey("id"))
                {
                    authorId = context.ActionArguments["id"] as int?;
                }
                if (context.ActionArguments.ContainsKey("author"))
                {
                    var author = context.ActionArguments["author"] as Author;
                    if (author != null)
                    {
                        authorId = author.Id;
                    }
                }
                if (authorId.HasValue)
                {
                    if (await _authorRepository.GetByIdAsync(authorId.Value) == null)
                    {
                        context.Result = new NotFoundObjectResult(authorId.Value);
                        return;
                    }
                }
                await next();
            }
        }
    }
}
