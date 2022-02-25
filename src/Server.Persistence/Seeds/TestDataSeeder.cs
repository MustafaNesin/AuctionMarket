using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Shared.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// ReSharper disable StringLiteralTypo

namespace AuctionMarket.Server.Persistence.Seeds;

// TODO: Seeding migration aracılığıyla yapılabilir
public class TestDataSeeder : IDataSeeder
{
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public TestDataSeeder(IAppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        => (_dbContext, _passwordHasher) = (dbContext, passwordHasher);

    public async Task EnsureSeedsAsync(CancellationToken cancellationToken = default)
    {
        await ((DbContext)_dbContext).Database.EnsureCreatedAsync(cancellationToken);

        if (await _dbContext.Users.AnyAsync(cancellationToken))
            return;

        #region Users

        var user1 = new User
        {
            UserName = "admin",
            FirstName = "Mustafa",
            LastName = "Nesin",
            Biography = "Gazi Üniversitesi Bilgisayar Mühendisliği",
            Balance = 10000.0,
            Role = Role.Administrator,
            CreatedAt = new DateTime(2022, 1, 1)
        };

        var user2 = new User
        {
            UserName = "ali",
            FirstName = "Ali",
            LastName = "Yılmaz",
            Balance = 5000.0,
            CreatedAt = new DateTime(2022, 1, 5)
        };

        var user3 = new User
        {
            UserName = "mehmet",
            FirstName = "Mehmet",
            LastName = "Demir",
            Balance = 1000.0,
            CreatedAt = new DateTime(2022, 1, 5)
        };

        var user4 = new User
        {
            UserName = "ahmet",
            FirstName = "Ahmet",
            LastName = "Şahin",
            Balance = 50000.0,
            CreatedAt = new DateTime(2022, 1, 5)
        };

        var user1Entry = await _dbContext.Users.AddAsync(user1, cancellationToken);
        user1Entry.Entity.PasswordHash = _passwordHasher.HashPassword(user1Entry.Entity, "password");

        var user2Entry = await _dbContext.Users.AddAsync(user2, cancellationToken);
        user2Entry.Entity.PasswordHash = _passwordHasher.HashPassword(user2Entry.Entity, "password");

        var user3Entry = await _dbContext.Users.AddAsync(user3, cancellationToken);
        user3Entry.Entity.PasswordHash = _passwordHasher.HashPassword(user3Entry.Entity, "password");

        var user4Entry = await _dbContext.Users.AddAsync(user4, cancellationToken);
        user4Entry.Entity.PasswordHash = _passwordHasher.HashPassword(user4Entry.Entity, "password");

        #endregion

        #region Auction 1

        var auction1 = new Auction
        {
            Title = "Auction 1",
            Description = "An example description for Auction 1.",
            StartingPrice = 100.0,
            MinBidIncrement = 20.0,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddHours(-4),
            StartsAt = DateTime.UtcNow.AddHours(-3),
            EndsAt = DateTime.UtcNow.AddHours(-2)
        };

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction1,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddHours(-2).AddMinutes(-45),
            Value = 100.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction1,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-2).AddMinutes(-30),
            Value = 150.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction1,
            CreatedBy = user4,
            CreatedAt = DateTime.UtcNow.AddHours(-2).AddMinutes(-15),
            Value = 200.0
        }, cancellationToken);

        await _dbContext.Auctions.AddAsync(auction1, cancellationToken);

        #endregion

        #region Auction 2

        var auction2 = new Auction
        {
            Title = "Auction 2",
            Description = "An example description for Auction 2.",
            StartingPrice = 20.0,
            MinBidIncrement = 5.0,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddHours(-4),
            StartsAt = DateTime.UtcNow.AddHours(-3),
            EndsAt = DateTime.UtcNow.AddHours(-2)
        };

        await _dbContext.Auctions.AddAsync(auction2, cancellationToken);

        #endregion

        #region Auction 3

        var auction3 = new Auction
        {
            Title = "Auction 3",
            Description = "An example description for Auction 3.",
            StartingPrice = 250.0,
            MinBidIncrement = 10.0,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddHours(-3),
            StartsAt = DateTime.UtcNow.AddHours(-2),
            EndsAt = DateTime.UtcNow.AddHours(-1)
        };

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction3,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-45),
            Value = 250.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction3,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-30),
            Value = 300.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction3,
            CreatedBy = user4,
            CreatedAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-15),
            Value = 350.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction3,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-5),
            Value = 500.0
        }, cancellationToken);

        await _dbContext.Auctions.AddAsync(auction3, cancellationToken);

        #endregion

        #region Auction 4

        var auction4 = new Auction
        {
            Title = "Auction 4",
            Description = "An example description for Auction 4.",
            StartingPrice = 300.0,
            MinBidIncrement = 15.0,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddHours(-3),
            StartsAt = DateTime.UtcNow.AddHours(-2),
            EndsAt = DateTime.UtcNow.AddHours(-1)
        };

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction4,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-1).AddMinutes(-10),
            Value = 350.0
        }, cancellationToken);

        await _dbContext.Auctions.AddAsync(auction4, cancellationToken);

        #endregion

        #region Auction 5

        var auction5 = new Auction
        {
            Title = "Auction 5",
            Description = "An example description for Auction 5.",
            StartingPrice = 500.0,
            MinBidIncrement = 5.0,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            StartsAt = DateTime.UtcNow.AddHours(-1),
            EndsAt = DateTime.UtcNow
        };

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction5,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddMinutes(-50),
            Value = 500.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction5,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddMinutes(-40),
            Value = 550.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction5,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddMinutes(-30),
            Value = 600.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction5,
            CreatedBy = user4,
            CreatedAt = DateTime.UtcNow.AddMinutes(-15),
            Value = 800.0
        }, cancellationToken);

        await _dbContext.Auctions.AddAsync(auction5, cancellationToken);

        #endregion

        #region Auction 6

        var auction6 = new Auction
        {
            Title = "Auction 6",
            Description = "An example description for Auction 6.",
            StartingPrice = 100.0,
            MinBidIncrement = 10.0,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            StartsAt = DateTime.UtcNow.AddHours(-1),
            EndsAt = DateTime.UtcNow
        };

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction6,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddMinutes(-45),
            Value = 100.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction6,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddMinutes(-30),
            Value = 110.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction6,
            CreatedBy = user4,
            CreatedAt = DateTime.UtcNow.AddMinutes(-15),
            Value = 150.0
        }, cancellationToken);

        await _dbContext.Bids.AddAsync(new Bid
        {
            Auction = auction6,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5),
            Value = 175.0
        }, cancellationToken);

        await _dbContext.Auctions.AddAsync(auction6, cancellationToken);

        #endregion

        #region Auction 7

        var auction7 = new Auction
        {
            Title = "Auction 7",
            Description = "An example description for Auction 7.",
            StartingPrice = 1000.0,
            MinBidIncrement = 10.0,
            CreatedBy = user1,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddHours(1)
        };

        await _dbContext.Auctions.AddAsync(auction7, cancellationToken);

        #endregion

        #region Auction 8

        var auction8 = new Auction
        {
            Title = "Auction 8",
            Description = "An example description for Auction 8.",
            StartingPrice = 2000.0,
            MinBidIncrement = 10.0,
            CreatedBy = user2,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddHours(1)
        };

        await _dbContext.Auctions.AddAsync(auction8, cancellationToken);

        #endregion

        #region Auction 9

        var auction9 = new Auction
        {
            Title = "Auction 9",
            Description = "An example description for Auction 9.",
            StartingPrice = 1500.0,
            MinBidIncrement = 10.0,
            CreatedBy = user3,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            StartsAt = DateTime.UtcNow,
            EndsAt = DateTime.UtcNow.AddHours(1)
        };

        await _dbContext.Auctions.AddAsync(auction9, cancellationToken);

        #endregion

        await _dbContext.SaveSeedChangesAsync(cancellationToken);
    }
}