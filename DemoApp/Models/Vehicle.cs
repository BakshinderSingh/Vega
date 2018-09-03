﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public bool IsRegistered { get; set; }
        public Contact Contact { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<VehicleFeature> Features { get; set; }
        public Feature Feature { set; get; }
        public ICollection<Photo> Photos { get; set; }
        public Vehicle()
        {
            Features = new Collection<VehicleFeature>();
            Photos = new Collection<Photo>();
        }
    }

    [Owned]
    public class Contact
    {
        [Required]
        [MaxLength(255)]
        public string ContactName { get; set; }
        [Required]
        [MaxLength(255)]
        public string ContactPhone { get; set; }
        [MaxLength(255)]
        public string ContactEmail { get; set; }
    }
}
