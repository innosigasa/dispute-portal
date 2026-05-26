using DisputePortal.Application.Feature.Dispute.Requests;
using FluentValidation;

namespace DisputePortal.Application.Feature.Dispute.Validators;

public class RaiseDisputeCommandValidator : AbstractValidator<RaiseDisputeCommand>
{
    public RaiseDisputeCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.ReasonCode).IsInEnum();
        RuleFor(x => x.Comments).MaximumLength(1000);
    }
}
