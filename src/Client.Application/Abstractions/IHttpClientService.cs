using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Client.Application.Abstractions;

public interface IHttpClientService
{
    Task<Response> GetAsync(string? requestUri, CancellationToken cancellationToken);
    Task<Response<TResponse>> GetAsync<TResponse>(string? requestUri, CancellationToken cancellationToken);
    Task<Response> PostAsync<TRequest>(string? requestUri, TRequest value, CancellationToken cancellationToken);

    Task<Response<TResponse>> PostAsync<TRequest, TResponse>(
        string? requestUri, TRequest value, CancellationToken cancellationToken);
}