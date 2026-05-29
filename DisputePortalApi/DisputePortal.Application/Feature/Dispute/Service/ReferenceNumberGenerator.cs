using DisputePortal.Application.Feature.Dispute.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DisputePortal.Application.Feature.Dispute.Service;

public class ReferenceNumberGenerator : IReferenceNumberGenerator
{
    private readonly IServiceProvider? _sp;
    private int _sequence;
    private int _initializedYear;
    private readonly object _lock = new();

    public ReferenceNumberGenerator(IServiceProvider sp) => _sp = sp;

    internal ReferenceNumberGenerator(int startSequence)
    {
        _sequence = startSequence;
        _initializedYear = DateTime.UtcNow.Year;
    }

    public string Generate()
    {
        var year = DateTime.UtcNow.Year;
        EnsureInitialized(year);
        var seq = Interlocked.Increment(ref _sequence);
        return $"DSP-{year}-{seq:D6}";
    }

    private void EnsureInitialized(int year)
    {
        if (_initializedYear == year) return;
        lock (_lock)
        {
            if (_initializedYear == year) return;
            using var scope = _sp!.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDisputeRepository>();
            var max = repo.GetMaxReferenceSequenceForYearAsync(year, CancellationToken.None)
                         .GetAwaiter().GetResult();
            _sequence = max;
            _initializedYear = year;
        }
    }
}
