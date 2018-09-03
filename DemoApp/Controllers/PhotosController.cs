using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.Controllers.Resources;
using DemoApp.Models;
using DemoApp.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DemoApp.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly PhotoSettings photoSettings;

        public PhotosController(IHostingEnvironment host,IVehicleRepository repository,IUnitOfWork unitOfWork, IMapper mapper,IOptionsSnapshot<PhotoSettings> options, IPhotoRepository photoRepository)
        {
            this.photoSettings = options.Value;
            this.host = host;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoRepository = photoRepository;
        }

        [HttpPost]
        public IActionResult Upload(int vehicleId,IFormFile file)
        {
            var vehicle = repository.GetVehicle(vehicleId,false);
            if (vehicle == null)
                return NotFound();
            if (file == null)
                return BadRequest("No File");
            if (file.Length == 0)
                return BadRequest("Empty File");
            if (file.Length > photoSettings.MaxBytes)
                return BadRequest("File max greater than maximum length");
            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest("Not a valid file");
            var uploadsFolderPath= Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            unitOfWork.Complete();
            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }

        [HttpGet]
        public IActionResult GetPhotos(int vehicleId)
        {
            return Ok(mapper.Map<List<Photo>, List<PhotoResource>>(photoRepository.GetPhotos(vehicleId).ToList()));
        }
    }
}