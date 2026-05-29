using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.StateMachine;
using FluentAssertions;

namespace DisputePortal.UnitTests;

public class DisputeStatusTransitionTests
{
    [Theory]
    [InlineData(DisputeStatus.Submitted, DisputeStatus.UnderReview)]
    [InlineData(DisputeStatus.UnderReview, DisputeStatus.Resolved)]
    [InlineData(DisputeStatus.UnderReview, DisputeStatus.Rejected)]
    public void IsValid_ValidTransitions_ReturnsTrue(DisputeStatus from, DisputeStatus to)
    {
        DisputeStatusTransition.IsValid(from, to).Should().BeTrue();
    }

    [Theory]
    [InlineData(DisputeStatus.Submitted, DisputeStatus.Resolved)]
    [InlineData(DisputeStatus.Submitted, DisputeStatus.Rejected)]
    [InlineData(DisputeStatus.Submitted, DisputeStatus.Submitted)]
    [InlineData(DisputeStatus.Resolved, DisputeStatus.Submitted)]
    [InlineData(DisputeStatus.Resolved, DisputeStatus.UnderReview)]
    [InlineData(DisputeStatus.Rejected, DisputeStatus.UnderReview)]
    [InlineData(DisputeStatus.UnderReview, DisputeStatus.Submitted)]
    public void IsValid_InvalidTransitions_ReturnsFalse(DisputeStatus from, DisputeStatus to)
    {
        DisputeStatusTransition.IsValid(from, to).Should().BeFalse();
    }

    [Fact]
    public void EnsureValid_InvalidTransition_Throws()
    {
        var act = () => DisputeStatusTransition.EnsureValid(DisputeStatus.Submitted, DisputeStatus.Resolved);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Submitted*Resolved*");
    }

    [Fact]
    public void EnsureValid_ValidTransition_DoesNotThrow()
    {
        var act = () => DisputeStatusTransition.EnsureValid(DisputeStatus.Submitted, DisputeStatus.UnderReview);
        act.Should().NotThrow();
    }

    [Fact]
    public void GetValidNext_ForSubmitted_ReturnsOnlyUnderReview()
    {
        var next = DisputeStatusTransition.GetValidNext(DisputeStatus.Submitted);
        next.Should().ContainSingle().Which.Should().Be(DisputeStatus.UnderReview);
    }

    [Fact]
    public void GetValidNext_ForResolved_IsEmpty()
    {
        DisputeStatusTransition.GetValidNext(DisputeStatus.Resolved).Should().BeEmpty();
    }
}
