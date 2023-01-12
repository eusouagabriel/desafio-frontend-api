using AutoMapper;
using Gabriel.DesafioFrontEnd.Api.Controllers.Resources;
using Gabriel.DesafioFrontEnd.Domain.Entities;

namespace Gabriel.DesafioFrontEnd.Api.Helpers.Configuration
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile(IConfiguration configuration) 
        {
            var baseUrl = configuration.GetSection("baseUrl").Value;
            CreateMap<Customer, CustomerGetResponse>();
            CreateMap<Customer, CustomerCamerasGetResponse>();

            CreateMap<Camera, CameraGetResponse>()
                .ForMember(x => x.ThumbnailUrl, d => d.MapFrom(x => $"{baseUrl}/assets/thumbs/{x.Thumbnail}"))
                .ForMember(x => x.VideoUrl, d => d.MapFrom(x => $"{baseUrl}/assets/videos/{x.Video}"));
        } 

    }
}
