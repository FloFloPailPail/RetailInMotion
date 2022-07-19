using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailInMotion.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() => Problem();
    }
}
