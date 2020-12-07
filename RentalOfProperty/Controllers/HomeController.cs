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

        public IActionResult Index()
        {
            FlatViewModel flatViewModel = new FlatViewModel { Flats = _dataRepository.GetFlats() };
            return View(flatViewModel);
        }

        [HttpGet]
        public IActionResult Announcement()
        {
            return View();
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
