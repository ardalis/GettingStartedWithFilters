using System;
using System.Threading;
using Filters101.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Filters101.Controllers
{
    public class RandomNumberController : Controller
    {
        [ServiceFilter(typeof(RandomNumberProviderFilter))]
        public int GetRandomNumber(int randomNumber)
        {
            return randomNumber;
        }
    }
}
