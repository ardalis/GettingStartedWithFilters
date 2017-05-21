using Filters101.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters101.Controllers
{
    [OrderLoggingActionFilter(Name = "Class Level Attribute")]
    [TypeFilter(typeof(OrderLoggingActionFilter), Arguments = new object[] { "Class Attribute" })]
    public class OrderingController : Controller
    {
        private readonly ILogger _logger;
        public OrderingController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OrderingController>();
        }

        [OrderLoggingActionFilter(Name = "Method Level Attribute")]
        public IActionResult Index()
        {
            return Content("Ordering Sample.");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"OnActionExecuting for {nameof(OrderingController)}");
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"OnActionExecuted for {nameof(OrderingController)}");
            base.OnActionExecuted(context);
        }
    }
}
