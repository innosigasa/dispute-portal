using DisputePortal.Application.Feature.Dispute.Service;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace DisputePortal.Tests;

public class ReferenceNumberGeneratorTests
{
    [Fact]
    public void Generate_MatchesExpectedFormat()
    {
        var sut = new ReferenceNumberGenerator(startSequence: 0);
        var result = sut.Generate();
        Regex.IsMatch(result, @"^DSP-\d{4}-\d{6}$").Should().BeTrue(because: $"'{result}' should match DSP-YYYY-NNNNNN");
    }

    [Fact]
    public void Generate_ProducesUniqueValues()
    {
        var sut = new ReferenceNumberGenerator(startSequence: 0);
        var results = Enumerable.Range(0, 10).Select(_ => sut.Generate()).ToList();
        results.Distinct().Should().HaveCount(10, "each generated reference should be unique");
    }

    [Fact]
    public void Generate_ContainsCurrentYear()
    {
        var sut = new ReferenceNumberGenerator(startSequence: 0);
        var result = sut.Generate();
        result.Should().Contain(DateTime.UtcNow.Year.ToString());
    }
}
