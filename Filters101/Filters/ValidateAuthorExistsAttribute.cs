using System.Linq;
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

        private class ValidateAuthorExistsFilterImpl : IActionFilter
        {
            private readonly IAuthorRepository _authorRepository;

            public ValidateAuthorExistsFilterImpl(IAuthorRepository authorRepository)
            {
                _authorRepository = authorRepository;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    var id = context.ActionArguments["id"] as int?;
                    if (id.HasValue)
                    {
                        if (_authorRepository.List().All(a => a.Id != id.Value))
                        {
                            context.Result = new NotFoundObjectResult(id.Value);
                        }
                    }
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}