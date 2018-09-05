using System.Collections.Generic;
using DemoApp.Models;

namespace DemoApp.Persistence
{
    public interface IPhotoRepository
    {
        IEnumerable<Photo> GetPhotos(int vehicleId);
    }
}