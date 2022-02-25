using AuctionMarket.Shared.Domain.Abstractions;

namespace AuctionMarket.Shared.Domain.DTOs;

public record AccountDto(IReadOnlyDictionary<string, string> Claims) : IDto;