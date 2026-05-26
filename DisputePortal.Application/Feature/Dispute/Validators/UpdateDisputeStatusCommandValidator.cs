using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Dispute.Requests;
using FluentValidation;

namespace DisputePortal.Application.Feature.Dispute.Validators;

public class UpdateDisputeStatusCommandValidator : AbstractValidator<UpdateDisputeStatusCommand>
{
    public UpdateDisputeStatusCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
        RuleFor(x => x.Notes)
            .NotEmpty()
            .When(x => x.NewStatus is DisputeStatus.Resolved or DisputeStatus.Rejected)
            .WithMessage("Notes are required when resolving or rejecting a dispute.");
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
