using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class RegisterAccountPage
{
    private readonly RegisterAccountCommand _command = new();
    private readonly RegisterAccountCommandValidator _validator = new();
    private MudForm _form = default!;
    private bool _isSubmitDisabled;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationStateTask;

        if (authenticationState.User.Identity?.IsAuthenticated == true)
            NavigationManager.NavigateTo("/");
    }

    private async Task OnSubmitAsync()
    {
        _isSubmitDisabled = true;
        await _form.Validate();

        if (!_form.IsValid)
        {
            _isSubmitDisabled = false;
            return;
        }

        var (isSuccess, problemDetails) = await Mediator.Send(_command);

        if (isSuccess)
        {
            NavigationManager.NavigateTo("/Account/Login?UserName=" + _command.UserName);
            Snackbar.Add("Successfully registered!", Severity.Success);
        }
        else
            ShowProblem(problemDetails, false);

        _isSubmitDisabled = false;
    }

    private string? PasswordMatch(string password) => password != _command.Password ? "Passwords don't match" : null;
}