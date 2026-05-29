using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;

namespace DisputePortal.UnitTests.Fakes
{
    public sealed class FakeDisputeRepository : IDisputeRepository
    {
        private readonly int _maxReferenceSequence;

        public FakeDisputeRepository(int maxReferenceSequence)
        {
            _maxReferenceSequence = maxReferenceSequence;
        }

        public Task<int> GetMaxReferenceSequenceForYearAsync(int year, CancellationToken ct)
            => Task.FromResult(_maxReferenceSequence);

        public Task<DisputeModel> CreateAsync(DisputeModel dispute, CancellationToken ct)
            => throw new NotSupportedException();

        public Task<PagedResult<DisputeListItemDto>> GetCustomerDisputesAsync(Guid customerId, 
            DisputeFilters filters, int page, int pageSize, CancellationToken ct)
            => throw new NotSupportedException();

        public Task<DisputeDetailDto?> GetByIdAsync(Guid id, CancellationToken ct)
            => throw new NotSupportedException();

        public Task<PagedResult<DisputeListItemDto>> GetAllPagedAsync(DisputeFilters filters, 
            int page, int pageSize, CancellationToken ct)
            => throw new NotSupportedException();

        public Task<DisputeDetailDto?> UpdateStatusAsync(Guid id, DisputeStatus newStatus, 
            string notes, string agentId, string agentRole, CancellationToken ct)
            => throw new NotSupportedException();

        public Task<DisputeSummaryStatsDto> GetSummaryStatsAsync(Guid customerId, CancellationToken ct)
            => throw new NotSupportedException();
    }
}
