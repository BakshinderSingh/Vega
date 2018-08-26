using System.Collections.Generic;
using DemoApp.Controllers.Resources;
using DemoApp.Models;

namespace DemoApp.Persistence
{
    public interface IVehicleRepository
    {
        List<Vehicle> GetVehicles(VehicleQuery filter);
        Vehicle GetVehicle(int id, bool includeRelated=true);
        void AddVehicle(Vehicle vehicle);
        void RemoveVehicle(Vehicle vehicle);
    }
}