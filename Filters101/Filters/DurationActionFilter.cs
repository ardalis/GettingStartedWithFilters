using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters101.Filters
{
    public class DurationActionFilter : IActionFilter
    {
        private Stopwatch _stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionDuration = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Stop();

            // add time to viewbag if viewresult
            ViewResult result = context.Result as ViewResult;
            if (result != null)
            {
                result.ViewData["actionDuration"] = actionDuration;
            }
        }
    }
}