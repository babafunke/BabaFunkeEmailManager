using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    {
        private readonly ISubscriber _subscriberManager;

        public SubscriberController(ISubscriber subscriberManager)
        {
            _subscriberManager = subscriberManager;
        }

        public async Task<IActionResult> Index()
        {
            var subscribers = await _subscriberManager.GetAllSubscribers();
            return View(subscribers);
        }

        public async Task<IActionResult> Detail(string email)
        {
            var subscriber = await _subscriberManager.GetSubscriberByEmail(email);
            if (subscriber == null)
            {
                return Content("Not Found!");
            }

            return View(subscriber);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subscriber subscriber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _subscriberManager.CreateSubscriber(subscriber);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string email)
        {
            var subscriber = await _subscriberManager.GetSubscriberByEmail(email);

            return View(subscriber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Subscriber subscriber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _subscriberManager.UpdateSubscriber(subscriber.Email, subscriber);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var response = await _subscriberManager.Unsubscribe(email);

            if (!response)
            {
                return View();
            }

            return RedirectToAction("Unsubscribe", "Home", new { email }); //navigate to wafunk publishing
        }

        public async Task<IActionResult> Delete(string email)
        {
            var response = await _subscriberManager.DeleteSubscriber(email);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
