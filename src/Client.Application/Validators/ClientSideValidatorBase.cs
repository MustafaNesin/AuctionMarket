using FluentValidation;

namespace AuctionMarket.Client.Application.Validators;

public class ClientSideValidatorBase<T> : AbstractValidator<T>
{
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var context = ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName));
        var result = await ValidateAsync(context);
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}