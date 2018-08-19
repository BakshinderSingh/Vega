using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DemoApp.Controllers.Resources;
using DemoApp.Models;
using DemoApp.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    public class MakesController : Controller
    {
        private readonly VegaDbContext context;
        private readonly IMapper mapper;

        public MakesController(VegaDbContext context ,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet("/api/makes")]
        public IEnumerable<MakeResource> Index()
        {
            var makes= context.Makes.Include(m => m.Models).ToList();
            return mapper.Map<List<Make>, List<MakeResource>>(makes);
        }

        [HttpGet("/api/features")]
        public IEnumerable<KeyValuePairResource> Features()
        {
            var makes = context.Features.ToList();
            return mapper.Map<List<Feature>, List<KeyValuePairResource>>(makes);
        }


    }
}