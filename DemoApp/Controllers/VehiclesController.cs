using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.Controllers.Resources;
using DemoApp.Models;
using DemoApp.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly VegaDbContext context;

        public VehiclesController(IMapper mapper, VegaDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        [HttpPost]
        public IActionResult CreateVehicle([FromBody]VehicleResource vehicle)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var vehicle1 = mapper.Map<VehicleResource, Vehicle>(vehicle);
            vehicle1.LastUpdate = DateTime.Now;
            context.Vehicles.Add(vehicle1);
            context.SaveChanges();
            var vehicle2 = mapper.Map<Vehicle, VehicleResource>(vehicle1);
            return Ok(vehicle2);
        }
    }
}