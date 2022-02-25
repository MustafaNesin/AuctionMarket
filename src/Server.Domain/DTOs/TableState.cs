using AuctionMarket.Server.Domain.Enumerations;

namespace AuctionMarket.Server.Domain.DTOs;

public record TableState(int Page, int PageSize, string? SortLabel, SortDirection SortDirection);