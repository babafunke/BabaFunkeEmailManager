using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Controllers
{
    [Authorize]
    public class NewsletterController : Controller
    {
        private readonly INewsletter _newsletterManager;

        public NewsletterController(INewsletter newsletterManager)
        {
            _newsletterManager = newsletterManager;
        }

        public async Task<IActionResult> Index()
        {
            var newsletters = await _newsletterManager.GetAllNewsletters();
            return View(newsletters);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var newsletter = await _newsletterManager.GetNewsletterById(id);
            if (newsletter == null)
            {
                return Content("Not Found!");
            }

            return View(newsletter);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Newsletter newsletter)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _newsletterManager.CreateNewsletter(newsletter);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var newsletter = await _newsletterManager.GetNewsletterById(id);

            return View(newsletter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Newsletter newsletter)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _newsletterManager.EditNewsletter(newsletter);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = await _newsletterManager.DeleteNewsletter(id);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}