using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Homepage.Controllers
{
    public class GamesController : Controller
    {
        public IActionResult Game()
        {
            return View();
        }
    }
}