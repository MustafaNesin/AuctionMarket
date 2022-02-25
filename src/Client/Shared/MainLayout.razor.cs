using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace AuctionMarket.Client.Shared;

public partial class MainLayout
{
    private const string IsDarkModeSetting = "IsDarkMode"; // Do not rename !!! (for serializing)
    private bool _isDarkMode;
    private bool _isDrawerOpen;

    private bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            LocalStorage.SetItem(IsDarkModeSetting, value);
        }
    }

    [Inject]
    private ISyncLocalStorageService LocalStorage { get; set; } = default!;

    protected override void OnInitialized() => _isDarkMode = LocalStorage.GetItem<bool>(IsDarkModeSetting);

    private void OnDrawerToggle() => _isDrawerOpen = !_isDrawerOpen;
}