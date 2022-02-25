using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.DTOs;
using AutoMapper;

namespace AuctionMarket.Server.Application.Profiles;

public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<Auction, AuctionDto>();
        CreateMap<AuctionDto, Auction>();
    }
}