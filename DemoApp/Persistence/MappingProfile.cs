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
            CreateMap<Make, MakeResource>()
                .ForMember(mk => mk.Id, opt => opt.MapFrom(mr => mr.Id))
                .ForMember(mk => mk.Name, opt => opt.MapFrom(mr => mr.Name))
                .ForMember(mk => mk.Models, opt => opt.MapFrom(mr=>mr.Models));

            CreateMap<KeyValuePairResource, Model>()
                .ForMember(m => m.Id, opt => opt.MapFrom(kv => kv.Id))
                .ForMember(m => m.Name, opt => opt.MapFrom(kv => kv.Name));

            CreateMap<Model,KeyValuePairResource > ()
                .ForMember(m => m.Id, opt => opt.MapFrom(kv => kv.Id))
                .ForMember(m => m.Name, opt => opt.MapFrom(kv => kv.Name));

            //API Resource to Domain
            CreateMap<ContactResource, Contact>()
                .ForMember(c => c.ContactEmail, opt => opt.MapFrom(cr => cr.Email))
                .ForMember(c => c.ContactName, opt => opt.MapFrom(cr => cr.Name))
                .ForMember(c => c.ContactPhone, opt => opt.MapFrom(cr => cr.Phone));
            CreateMap<Contact, ContactResource>()
                .ForMember(c => c.Email, opt => opt.MapFrom(cr => cr.ContactEmail))
                .ForMember(c => c.Name, opt => opt.MapFrom(cr => cr.ContactName))
                .ForMember(c => c.Phone, opt => opt.MapFrom(cr => cr.ContactPhone));
            CreateMap<SaveVehicleResource, Vehicle>()

                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Contact, opt => opt.MapFrom(vr => Mapper.Map<ContactResource, Contact>(vr.Contact)))
                //.ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(id => new VehicleFeature { FeatureId = id })))
                .ForMember(v => v.Features, opt =>opt.Ignore())
                .AfterMap((vr, v) =>
                {
                    //removed removed features
                    //List<VehicleFeature> removeFeatures = new List<VehicleFeature>();
                    //foreach (var f in v.Features)
                    //    if (!vr.Features.Contains(f.FeatureId))
                    //        removeFeatures.Add(f);
                    var removeFeatures = v.Features.Where(x => !vr.Features.Contains(x.FeatureId));
                    foreach (var rf in removeFeatures)
                        v.Features.Remove(rf);
                    //add new features
                    //foreach (var id in vr.Features)
                    //    if (!v.Features.Any(vf => vf.FeatureId == id))
                    //        v.Features.Add(new VehicleFeature { FeatureId = id });
                    var addedFeatures=vr.Features.Where(x => !v.Features.Any(vf => vf.FeatureId == x)).Select(y => new VehicleFeature { FeatureId = y });
                    foreach (var rf in addedFeatures)
                        v.Features.Add(rf);
                });
               

            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(v => v.Contact, opt => opt.MapFrom(vr => Mapper.Map<Contact, ContactResource>(vr.Contact)))
                .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(vf=>vf.FeatureId)));

            CreateMap<Vehicle,VehicleResource>()
                .ForMember(v=>v.Make,opt=>opt.MapFrom(vr=>vr.Model.Make))
                .ForMember(v => v.Contact, opt => opt.MapFrom(vr => Mapper.Map<Contact, ContactResource>(vr.Contact)))
                .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(vf =>new KeyValuePairResource { Id = vf.FeatureId, Name = vf.Feature.Name })));
        }

    }
}
