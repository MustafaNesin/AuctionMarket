using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using AuctionMarket.Client.Domain.Queries;
using AuctionMarket.Client.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class LoginAccountPage
{
    private readonly LoginAccountCommand _command = new();
    private readonly LoginAccountCommandValidator _validator = new();
    private MudForm _form = default!;
    private bool _isSubmitDisabled;

    [Parameter]
    [SupplyParameterFromQuery]
    public string? UserName { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    [Inject]
    private IHubConnectionService HubConnection { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var authenticationState = await AuthenticationStateTask;

        if (authenticationState.User.Identity?.IsAuthenticated == true)
            NavigationManager.NavigateTo("/");

        _command.UserName = UserName!;
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

        var (isSuccess, account, problemDetails) = await Mediator.Send(_command);

        if (isSuccess)
        {
            var (isSuccess2, user, problemDetails2) =
                await Mediator.Send(new GetUserQuery(_command.UserName));

            if (isSuccess2)
            {
                var authState = account.ConvertToAuthenticationState();

                StateContainer.Authentication.SetAuthenticationState(Task.FromResult(authState));
                StateContainer.User = user;

                await HubConnection.StartAsync();
                NavigationManager.NavigateTo("/");
                Snackbar.Add("Logged in successfully!", Severity.Success);
            }
            else
                ShowProblem(problemDetails2, false);
        }
        else
            ShowProblem(problemDetails, false);

        _isSubmitDisabled = false;
    }
}