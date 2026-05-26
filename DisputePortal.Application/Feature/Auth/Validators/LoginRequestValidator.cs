using DisputePortal.Application.Feature.Auth.Requests;
using FluentValidation;

namespace DisputePortal.Application.Feature.Auth.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
