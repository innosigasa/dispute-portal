using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Validators;
using FluentAssertions;

namespace DisputePortal.UnitTests;

public class RaiseDisputeCommandValidatorTests
{
    private readonly RaiseDisputeCommandValidator _sut = new();

    [Fact]
    public async Task Valid_Command_PassesValidation()
    {
        var cmd = new RaiseDisputeCommand(Guid.NewGuid(), DisputeReasonCode.UnauthorisedTransaction, "Some comments");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Empty_TransactionId_FailsValidation()
    {
        var cmd = new RaiseDisputeCommand(Guid.Empty, DisputeReasonCode.Other, "");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TransactionId");
    }

    [Fact]
    public async Task Comments_ExceedingMaxLength_FailsValidation()
    {
        var cmd = new RaiseDisputeCommand(Guid.NewGuid(), DisputeReasonCode.Other, new string('x', 1001));
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Comments");
    }
}

public class UpdateDisputeStatusCommandValidatorTests
{
    private readonly UpdateDisputeStatusCommandValidator _sut = new();

    [Fact]
    public async Task Resolved_WithoutNotes_FailsValidation()
    {
        var cmd = new UpdateDisputeStatusCommand(Guid.NewGuid(), DisputeStatus.Resolved, "");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Notes");
    }

    [Fact]
    public async Task Rejected_WithoutNotes_FailsValidation()
    {
        var cmd = new UpdateDisputeStatusCommand(Guid.NewGuid(), DisputeStatus.Rejected, "");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task UnderReview_WithoutNotes_PassesValidation()
    {
        var cmd = new UpdateDisputeStatusCommand(Guid.NewGuid(), DisputeStatus.UnderReview, "");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Resolved_WithNotes_PassesValidation()
    {
        var cmd = new UpdateDisputeStatusCommand(Guid.NewGuid(), DisputeStatus.Resolved, "Full refund issued after investigation.");
        var result = await _sut.ValidateAsync(cmd);
        result.IsValid.Should().BeTrue();
    }
}
