using System.Linq;
using System.Threading.Tasks;
using Filters101.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters101.Filters
{
    public class ValidateAuthorExistsAttribute : TypeFilterAttribute
    {
        public ValidateAuthorExistsAttribute():base(typeof(ValidateAuthorExistsFilterImpl))
        {
        }

        private class ValidateAuthorExistsFilterImpl : IAsyncActionFilter
        {
            private readonly IAuthorRepository _authorRepository;

            public ValidateAuthorExistsFilterImpl(IAuthorRepository authorRepository)
            {
                _authorRepository = authorRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if ((await _authorRepository.ListAsync()).All(a => a.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}