using AuctionMarket.Client.Application.Validators;
using AuctionMarket.Client.Domain.Commands;
using MudBlazor;

namespace AuctionMarket.Client.Pages;

public partial class DepositMoneyPage
{
    private readonly DepositMoneyCommand _command = new() { Value = 10.0 };
    private readonly DepositMoneyCommandValidator _validator = new();
    private MudForm _form = default!;
    private bool _isSubmitDisabled;

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
            Snackbar.Add($"Successfully deposited: {_command.Value:N2}$", Severity.Success);
        else
            ShowProblem(problemDetails, false);

        _isSubmitDisabled = false;
    }
}