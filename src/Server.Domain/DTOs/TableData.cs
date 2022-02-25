namespace AuctionMarket.Server.Domain.DTOs;

public record TableData<T>(IEnumerable<T> Items, int TotalItems);