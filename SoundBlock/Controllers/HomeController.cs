using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SoundBlock.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
