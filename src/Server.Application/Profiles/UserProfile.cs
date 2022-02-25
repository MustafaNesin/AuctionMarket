using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.DTOs;
using AutoMapper;

namespace AuctionMarket.Server.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}