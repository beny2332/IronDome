using IronDome.Models;
using IronDome.DAL;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IronDome.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Attacks() 
        {
            return View(Data.Get.Threats.ToList());
        }

        public IActionResult CreateAttack() 
        {
            return View();
        }

        public IActionResult DefenceSystem() 
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
