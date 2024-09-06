using Journer.Repository;
using Journey.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JoyfulJourney.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository repository;

        public AdminController(IRepository repository)
        {
            this.repository = repository;
        }
        // GET: AdminController
     
        public ActionResult GetData()
        {
            var obj = repository.getAdminDestination();
            return View(obj);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AdminCrud()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateDest(int id)
        {
            var obj = repository.getAdminDestination().Find(m => m.DestinationId == id);
            if (obj == null)
            {
                return NotFound(); // Handle the case when the item is not found
            }
            // Convert GetAdminDestinationDTO to UpdateAdminDestinationDTO if needed
            var updateDto = new UpdateAdminDestinationDTO
            {
                DestinationId = obj.DestinationId,
                Name = obj.Name,
                Description = obj.Description,
                Country = obj.Country
            };
            return View(updateDto);
        }


        [HttpPost]
        public IActionResult UpdateDesst(UpdateAdminDestinationDTO updateAdminDestinationDTO)
        {
            repository.UpdateDest(updateAdminDestinationDTO);
            return RedirectToAction("GetData", "Admin");
        }


       

        [HttpGet]
        public ActionResult AddDestination()
        {

            return View();
        }

        public ActionResult AddDestination(AddAdminDestinationDTO destinationDTO)
        {
            repository.AddDest(destinationDTO);
            return RedirectToAction("GetData","Admin");
        }

        public ActionResult AddPackage()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
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

        // GET: AdminController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            repository.DeleteDest(id);
            return RedirectToAction("GetData", "Admin");
        }

        // POST: AdminController/Delete/5

    }
}
