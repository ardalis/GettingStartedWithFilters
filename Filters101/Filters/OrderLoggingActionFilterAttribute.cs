using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters101.Filters
{
    public class OrderLoggingActionFilterAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var loggerFactory =
                context.HttpContext.RequestServices.GetService(typeof (ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory != null)
            {
                var logger = loggerFactory.CreateLogger<OrderLoggingActionFilterAttribute>();
                logger.LogInformation($"OnActionExecuting for {Name}");
            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var loggerFactory =
                context.HttpContext.RequestServices.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            if (loggerFactory != null)
            {
                var logger = loggerFactory.CreateLogger<OrderLoggingActionFilterAttribute>();
                logger.LogInformation($"OnActionExecuted for {Name}");
            }
            base.OnActionExecuted(context);
        }
    }
}