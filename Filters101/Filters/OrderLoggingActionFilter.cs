using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters101.Filters
{
    public class OrderLoggingActionFilter : IActionFilter
    {
        private ILogger<OrderLoggingActionFilter> _logger;
        public OrderLoggingActionFilter(ILoggerFactory loggerFactory, string name)
        {
            _logger = loggerFactory.CreateLogger<OrderLoggingActionFilter>();

            // can't set properties when using TypeFilter to create instance of filter
            Name = name;

        }

        public string Name { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"OnActionExecuting for {Name}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"OnActionExecuted for {Name}");
        }
    }
}