using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IRequest _requestManager;
        private readonly INewsletter _newsletterManager;
        private readonly ISubscriber _subscriberManager;

        public RequestController(
            IRequest requestManager,
            INewsletter newsletterManager,
            ISubscriber subscriberManager)
        {
            _requestManager = requestManager;
            _newsletterManager = newsletterManager;
            _subscriberManager = subscriberManager;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _requestManager.GetAllRequests();
            return View(requests);
        }

        public async Task<ActionResult> Create()
        {
            var newsletters = await _newsletterManager.GetAllNewsletters();
            var subscribers = await _subscriberManager.GetAllSubscribers();
            var selectListItems = GetSelectListItems(newsletters, subscribers);

            ViewBag.Newsletters = new SelectList(selectListItems.newsletterItems, "Value", "Text");
            ViewBag.SubscriberCategories = new SelectList(selectListItems.subscriberItems, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestHeader request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _requestManager.CreateRequest(request);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = await _requestManager.DeleteRequest(id);

            if (!response)
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        private (IEnumerable<SelectListItem> newsletterItems, IEnumerable<SelectListItem> subscriberItems)
            GetSelectListItems(IEnumerable<Newsletter> newsletters, IEnumerable<Subscriber> subscribers)
        {
            var newsletterItems = new List<SelectListItem>(newsletters
                .Where(n => n.IsEnabled)
                .Select(n => new SelectListItem
            {
                Text = n.NewsletterId,
                Value = n.NewsletterId
            }));

            var subscriberItems = new List<SelectListItem>(subscribers
                .Select(s => s.SubCategory).Distinct()
                .Select(category => new SelectListItem
            {
                Text = category,
                Value = category
            }));

            return (newsletterItems, subscriberItems);
        }
    }
}