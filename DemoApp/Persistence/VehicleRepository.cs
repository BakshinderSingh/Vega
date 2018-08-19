using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext context;

        public VehicleRepository(VegaDbContext context)
        {
            this.context = context;
        }

        public Vehicle GetVehicle(int id,bool includeRelated=true)
        {
            if (!includeRelated)
                return context.Vehicles.Find(id);
            return context.Vehicles
                .Include(v => v.Model).ThenInclude(m => m.Make)
                .Include(v => v.Features).ThenInclude(vf => vf.Feature)
                .SingleOrDefault(v => v.Id == id);
        }
        public void AddVehicle(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
        }
        public void RemoveVehicle(Vehicle vehicle)
        {
            context.Vehicles.Remove(vehicle);
        }
    }
}
