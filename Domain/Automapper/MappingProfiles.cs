using AutoMapper;
using HotelApp1.DTO;
using HotelApp1.Entities.Models;
using static HotelApp1.DTO.ClientDto;
namespace HotelApp1.Domain.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<BrowsingData, DataDto>().ReverseMap();
            CreateMap<ExtraService, ServicesDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<ReservationService, ReservationServicesDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<RoomType, RoomtypeDto>().ReverseMap();
			CreateMap<ClientRegistrationDTO, Client>().ReverseMap(); 
            CreateMap<ClientLoginDTO, Client>().ReverseMap();

		}
    }
}
