using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AuctionMarket.Server.Application.Validators;

// TODO: İşe yaramadığından emin olunduktan sonra silinebilir (Transient)
public class ValidatorExceptionInterceptor : IValidatorInterceptor
{
    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        => commonContext;

    public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
        ValidationResult result)
    {
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        return result;
    }
}