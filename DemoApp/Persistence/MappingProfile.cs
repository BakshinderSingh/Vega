using AutoMapper;
using DemoApp.Controllers.Resources;
using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Persistence
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){
            //API Resource to Domain
            CreateMap<ContactResource, Contact>()
                .ForMember(c => c.ContactEmail, opt => opt.MapFrom(cr => cr.Email))
                .ForMember(c => c.ContactName, opt => opt.MapFrom(cr => cr.Name))
                .ForMember(c => c.ContactPhone, opt => opt.MapFrom(cr => cr.Phone));
            CreateMap<Contact, ContactResource>()
                .ForMember(c => c.Email, opt => opt.MapFrom(cr => cr.ContactEmail))
                .ForMember(c => c.Name, opt => opt.MapFrom(cr => cr.ContactName))
                .ForMember(c => c.Phone, opt => opt.MapFrom(cr => cr.ContactPhone));
            CreateMap<VehicleResource, Vehicle>()
                .ForMember(v => v.Contact, opt => opt.MapFrom(vr => Mapper. Map<ContactResource, Contact>(vr.Contact)))
                .ForMember(v=>v.Features,opt=>opt.MapFrom(vr=>vr.Features.Select(id=>new VehicleFeature {FeatureId=id })));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(v => v.Contact, opt => opt.MapFrom(vr => Mapper.Map<Contact, ContactResource>(vr.Contact)))
                .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(vf=>vf.FeatureId)));
        }

    }
}
