namespace AuctionMarket.Shared.Domain.DTOs;

public record Response<TData>(bool IsSuccess, TData Data, ProblemDetails ProblemDetails)
    : Response(IsSuccess, ProblemDetails);