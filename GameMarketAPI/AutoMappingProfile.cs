using System.Linq;
using AutoMapper;
using game_market_API.Models;
using game_market_API.ViewModels;

namespace game_market_API
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Game, GameViewModel>()
                .ForMember(dest => dest.Vendor,
                    opt =>
                        opt.MapFrom(src => src.Vendor.Username))
                .ForMember(dest => dest.AvailableKeysCount, 
                    opt => 
                        opt.MapFrom(src => src.GameKeys.Count(key => !key.IsActivated)));
            
            CreateMap<GameKey, GameKeyViewModel>()
                .ForMember(dest => dest.Game, 
                    opt => 
                        opt.MapFrom(src => src.Game.Name));

            CreateMap<PaymentSession, PaymentSessionViewModel>()
                .ForMember(dest => dest.Client,
                    opt =>
                        opt.MapFrom(src => src.Client.Username))
                .ForMember(dest => dest.Game,
                    opt =>
                        opt.MapFrom(src => src.GameKeys.First().Game.Name))
                .ForMember(dest => dest.KeysCount,
                    opt =>
                        opt.MapFrom(src => src.GameKeys.Count))
                .ForMember(dest => dest.Total,
                    opt => 
                        opt.MapFrom(src => src.GameKeys.First().Game.Price * src.GameKeys.Count));
        }
    }
}