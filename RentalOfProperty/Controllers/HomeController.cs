using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentalOfProperty.Data;
using RentalOfProperty.Models;
using RentalOfProperty.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RentalOfProperty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        public readonly IDataRepository _dataRepository;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IDataRepository dataRepository)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;
            _dataRepository = dataRepository;
        }
        
        public IActionResult Index(FlatViewModel model)
        {
            SetDefaultValues(model);
            model = new FlatViewModel { Flats = _dataRepository.GetFlats(model) };
            return View(model);
        }

        public void SetDefaultValues(FlatViewModel model)
        {
            if(model.CitySearch is null)
            {
                model.CitySearch = string.Empty;
            }
            if(model.FromPrice is null)
            {
                model.FromPrice = 0;
            }
            if (model.ToPrice is null)
            {
                model.ToPrice = double.MaxValue;
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Announcement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Announcement(FlatViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                model.FlatPicturePath = uniqueFileName;
                model.OwnerId = _dataRepository.GetUserByEmail(User.Identity.Name).Id;
                _dataRepository.InsertFlat(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Flat flat = _dataRepository.GetFlat(id);
            if(flat == null)
            {
                return NotFound();
            }
            FlatViewModel model = new FlatViewModel
            {
                Balcony = flat.Balcony,
                AdditionalInformation = flat.AdditionalInformation,
                City = flat.Address.City.Name,
                Street = flat.Address.Street.Name,
                FlatNumber = flat.Address.FlatNumber,
                FlatPicturePath = flat.FlatPicture,
                PriceForMonth = flat.PriceForMonth,
                HouseNumber = flat.Address.HouseNumber,
                NumberOfRooms = flat.NumberOfRooms,
                Header = flat.Header,
                TotalArea = flat.TotalArea,
                TypeOfHouse = flat.TypeOfHouse,     
                OwnerId = flat.OwnerId,
                Owner = flat.Owner
            };            
            return View(model);
        }
        
        [HttpGet]
        public IActionResult MyAnnouncements(FlatViewModel model)
        {            
            SetDefaultValues(model);
            model = new FlatViewModel { Flats = _dataRepository.GetFlats(model).Where(m => m.Owner.Id 
            == _dataRepository.GetUserByEmail(User.Identity.Name).Id) };
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dataRepository.DeleteFlat(id);
            return RedirectToAction("MyAnnouncements");
        } 

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string UploadedFile(FlatViewModel model)
        {
            string uniqueFileName = null;
            if (model.FlatPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FlatPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FlatPicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
