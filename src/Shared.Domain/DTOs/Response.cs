namespace AuctionMarket.Shared.Domain.DTOs;

public record Response(bool IsSuccess, ProblemDetails ProblemDetails);