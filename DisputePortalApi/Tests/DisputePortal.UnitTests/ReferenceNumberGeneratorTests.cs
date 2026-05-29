using System.Text.RegularExpressions;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Service;
using DisputePortal.UnitTests.Fakes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace DisputePortal.UnitTests;

public class ReferenceNumberGeneratorTests
{
    [Fact]
    public void Generate_MatchesExpectedFormat()
    {
        var sut = CreateSut(maxReferenceSequence: 0);
        var result = sut.Generate();
        Regex.IsMatch(result, @"^DSP-\d{4}-\d{6}$").Should()
            .BeTrue(because: $"'{result}' should match DSP-YYYY-NNNNNN");
    }

    [Fact]
    public void Generate_ProducesUniqueValues()
    {
        var sut = CreateSut(maxReferenceSequence: 0);
        var results = Enumerable.Range(0, 10).Select(_ => sut.Generate()).ToList();
        results.Distinct().Should().HaveCount(10, "each generated reference should be unique");
    }

    [Fact]
    public void Generate_ContainsCurrentYear()
    {
        var sut = CreateSut(maxReferenceSequence: 0);
        var result = sut.Generate();
        result.Should().Contain(DateTime.UtcNow.Year.ToString());
    }

    [Fact]
    public void Generate_StartsAfterCurrentYearMaxSequenceFromRepository()
    {
        var sut = CreateSut(maxReferenceSequence: 41);

        var result = sut.Generate();

        result.Should().EndWith("000042");
    }

    private static ReferenceNumberGenerator CreateSut(int maxReferenceSequence)
    {
        var services = new ServiceCollection();
        services.AddScoped<IDisputeRepository>(_ => new FakeDisputeRepository(maxReferenceSequence));

        return new ReferenceNumberGenerator(services.BuildServiceProvider());
    }

}
