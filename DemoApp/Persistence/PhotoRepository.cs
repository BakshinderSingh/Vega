using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly VegaDbContext context;

        public PhotoRepository(VegaDbContext context)
        {
            this.context = context;
        }
        public  IEnumerable<Photo> GetPhotos(int vehicleId)
        {
            return context.Photos.Where(p => p.VehicleId == vehicleId).ToList();
        }
    }
}
