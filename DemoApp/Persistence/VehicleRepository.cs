using DemoApp.Controllers.Resources;
using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public List<Vehicle> GetVehicles(VehicleQuery queryObject)
        {
            var query = context.Vehicles.Include(v => v.Model).ThenInclude(m => m.Make)
                .Include(v => v.Features).ThenInclude(vf => vf.Feature).AsQueryable();
            if (queryObject.MakeId.HasValue)
                query = query.Where(x => x.Model.Make.Id == queryObject.MakeId);
            if (queryObject.SortBy != null)
                query = SortByFilter(queryObject, query);
            query = query.Skip((queryObject.Page-1) * queryObject.PageSize).Take(queryObject.PageSize);
            return query.ToList();
        }

        private IQueryable<Vehicle> SortByFilter(VehicleQuery queryObject, IQueryable<Vehicle> query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.Contact.ContactName,
                ["id"] = v => v.Id
            };
            {
                if (queryObject.IsSortAscending)
                    return query.OrderBy(columnsMap[queryObject.SortBy]);
                else
                    return query.OrderByDescending(columnsMap[queryObject.SortBy]);
            }
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
