using System.Net.Http.Json;
using System.Text.Json;
using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Shared.Domain.DTOs;

namespace AuctionMarket.Client.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<Response> GetAsync(
        string? requestUri, CancellationToken cancellationToken)
    {
        using var httpResponse = await _httpClient.GetAsync(
            requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return await ConvertHttpResponseAsync(httpResponse, cancellationToken);
    }

    public async Task<Response<TResponse>> GetAsync<TResponse>(
        string? requestUri, CancellationToken cancellationToken)
    {
        using var httpResponse = await _httpClient.GetAsync(
            requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return await ConvertHttpResponseAsync<TResponse>(httpResponse, cancellationToken);
    }

    public async Task<Response> PostAsync<TRequest>(
        string? requestUri, TRequest value, CancellationToken cancellationToken)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(requestUri, value, cancellationToken);
        return await ConvertHttpResponseAsync(httpResponse, cancellationToken);
    }

    public async Task<Response<TResponse>> PostAsync<TRequest, TResponse>(
        string? requestUri, TRequest value, CancellationToken cancellationToken)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(requestUri, value, cancellationToken);
        return await ConvertHttpResponseAsync<TResponse>(httpResponse, cancellationToken);
    }

    private static async Task<Response> ConvertHttpResponseAsync(
        HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        if (httpResponse.IsSuccessStatusCode)
            return new Response(true, default!);

        var problem = await httpResponse.Content
            .ReadFromJsonAsync<ProblemDetails>(default(JsonSerializerOptions), cancellationToken)
            .ConfigureAwait(false);

        return new Response(false, problem!);
    }

    private static async Task<Response<TResponse>> ConvertHttpResponseAsync<TResponse>(
        HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        if (httpResponse.IsSuccessStatusCode)
        {
            var account = await httpResponse.Content
                .ReadFromJsonAsync<TResponse>(default(JsonSerializerOptions), cancellationToken)
                .ConfigureAwait(false);

            return new Response<TResponse>(true, account!, default!);
        }

        var problem = await httpResponse.Content
            .ReadFromJsonAsync<ProblemDetails>(default(JsonSerializerOptions), cancellationToken)
            .ConfigureAwait(false);

        return new Response<TResponse>(false, default!, problem!);
    }
}