using System;
using System.Threading;
using Filters101.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Filters101.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [TypeFilter(typeof(DurationActionFilter))]
        public IActionResult RandomTime()
        {
            _logger.LogInformation(nameof(RandomTime));
            Thread.Sleep(new Random().Next(2000));
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
