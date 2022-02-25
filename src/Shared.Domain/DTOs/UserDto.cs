using System.Text.Json.Serialization;
using AuctionMarket.Shared.Domain.Abstractions;
using AuctionMarket.Shared.Domain.ValueObjects;

namespace AuctionMarket.Shared.Domain.DTOs;

public record UserDto : IDto
{
    public double? Balance { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Role? Role { get; set; }

    [JsonIgnore]
    public string? Initials => FirstName is null && LastName is null
        ? UserName?.FirstOrDefault().ToString()
        : FirstName?.FirstOrDefault().ToString() + LastName?.FirstOrDefault();

    [JsonIgnore]
    public string? FullName => FirstName is null && LastName is null ? UserName : FirstName + " " + LastName;

    #region Mutable

    public string? Biography { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }

    #endregion
}