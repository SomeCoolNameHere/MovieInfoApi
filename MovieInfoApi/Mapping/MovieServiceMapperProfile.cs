using AutoMapper;
using MovieInfoApi.Models;

namespace MovieInfoApi.Mapping;

public class MovieServiceMapperProfile : Profile
{
    public MovieServiceMapperProfile()
    {
        CreateMap<ClientRequestInfo, ClientRequestInfoResponse>(MemberList.Destination)
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s._id.ToString()));
    }
}