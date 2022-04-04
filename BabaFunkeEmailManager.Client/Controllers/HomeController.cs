using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReport _report;

        public HomeController(ILogger<HomeController> logger, IReport report)
        {
            _logger = logger;
            _report = report;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Unsubscribe(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Reports()
        {
            var reports = await _report.GetAllEmailResponse();
            return View(reports);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}