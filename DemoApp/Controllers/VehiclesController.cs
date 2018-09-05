using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.Controllers.Resources;
using DemoApp.Models;
using DemoApp.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IMapper mapper,IVehicleRepository repository,IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
            //throw new Exception();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var model = context.Models.Find(vehicleResource.ModelId);
            //if (model == null)
            //{
            //    ModelState.AddModelError("ModelId", "Invalid modelId");
            //    return BadRequest(ModelState);
            //}
            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            repository.AddVehicle(vehicle);
            unitOfWork.Complete();
            vehicle = repository.GetVehicle(vehicle.Id);
            var vehicleResourceOut = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(vehicleResourceOut);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVehicle(int id, [FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle = repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound();
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;
            unitOfWork.Complete();
            vehicle = repository.GetVehicle(vehicle.Id);
            var vehicleResourceOut = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(vehicleResourceOut);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = repository.GetVehicle(id,includeRelated:false);
            if (vehicle == null)
                return NotFound();
            repository.RemoveVehicle(vehicle);
            unitOfWork.Complete();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public IActionResult GetVehicles(int id)
        {
            var vehicle = repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound();
            var vehicleResource= mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }


        [HttpGet]
        public IActionResult GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var vehicles = repository.GetVehicles(filter);
            if (vehicles.Count() == 0)
                return NotFound();
            return Ok(mapper.Map<List<Vehicle>, List<VehicleResource>>(vehicles));
        }
    }
}