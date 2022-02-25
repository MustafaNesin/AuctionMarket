using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.DTOs;
using AutoMapper;

namespace AuctionMarket.Server.Application.Profiles;

public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<BidDto, Bid>();
    }
}