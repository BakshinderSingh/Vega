using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
    public class VegaDbContext:DbContext
    {
        public VegaDbContext(DbContextOptions<VegaDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<Make> Makes { get; set; }

    }
}
