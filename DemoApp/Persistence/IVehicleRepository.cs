using DemoApp.Models;

namespace DemoApp.Persistence
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicle(int id, bool includeRelated=true);
        void AddVehicle(Vehicle vehicle);
        void RemoveVehicle(Vehicle vehicle);
    }
}