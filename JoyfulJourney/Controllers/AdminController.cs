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
        [HttpGet]

        public ActionResult GetPackages()
        {
            var obj = repository.getAdminPackages();
            if (obj == null || !obj.Any())
            {
                // Add some logging here to see if the list is empty
                throw new Exception("No data returned from the database");
            }
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

        [HttpPost]
        public async Task<IActionResult> AddDestination(AddAdminDestinationDTO addAdminDestinationDTO )
        {
            if (addAdminDestinationDTO.IMAGE != null && addAdminDestinationDTO.IMAGE.Length > 0)
            {
                // Determine the file path and ensure it exists
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Destination");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(addAdminDestinationDTO.IMAGE.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file synchronously
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    addAdminDestinationDTO.IMAGE.CopyTo(stream); // Synchronously copy the file
                }

                addAdminDestinationDTO.ImageURl = $"/Destination/{fileName}"; // Set image path in DTO
            }

            // Add the breakfast dish
            repository.AddDest(addAdminDestinationDTO);

            return RedirectToAction("GetData", "Admin");
        }
        [HttpGet]
        public ActionResult AddPackages()
        {

            return View();
        }
        public ActionResult AddPackages(AddPackageAdmin addPackageAdmin)
        {
            repository.AddPackageAdmin(addPackageAdmin);
            return RedirectToAction("GetPackage", "Admin");
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

        [HttpGet]
        public ActionResult DeletePack(int id)
        {
            repository.DeletePackageAdmin(id);
            return RedirectToAction("GetData", "Admin");
        }

        // POST: AdminController/Delete/5

    }
}
