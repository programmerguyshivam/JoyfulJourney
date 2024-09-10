using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JoyfulJourney.Controllers
{
    public class BookCustomer : Controller
    {
        // GET: BookCustomer
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookCustomer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookCustomer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookCustomer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Custome(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookCustomer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookCustomer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookCustomer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookCustomer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
