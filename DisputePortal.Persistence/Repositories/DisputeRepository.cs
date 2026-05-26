using DisputePortal.Application.Common;
using DisputePortal.Application.Domain.Entities;
using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.StateMachine;
using DisputePortal.Application.Feature.Account.Results;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;
using DisputePortal.Application.Feature.Transaction.Results;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Repositories;

public class DisputeRepository(DisputePortalDbContext ctx) : IDisputeRepository
{
    public async Task<Dispute> CreateAsync(Dispute dispute, CancellationToken ct)
    {
        ctx.Disputes.Add(dispute);
        await ctx.SaveChangesAsync(ct);
        return dispute;
    }

    public async Task<PagedResult<DisputeListItemDto>> GetCustomerDisputesAsync(
        Guid customerId, DisputeFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var query = ctx.Disputes
            .AsNoTracking()
            .Where(d => d.CustomerId == customerId);

        query = ApplyFilters(query, filters);
        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(d => d.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DisputeListItemDto(
                d.Id, d.ReferenceNumber,
                d.Transaction.TransactionDate, d.Transaction.Amount,
                d.Reason.ToString(), d.Status.ToString(),
                d.SubmittedAt, d.UpdatedAt,
                null,
                d.Transaction.Account.AccountType.ToString(),
                d.Transaction.Account.AccountNumber
            ))
            .ToListAsync(ct);

        return new PagedResult<DisputeListItemDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<DisputeDetailDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await ctx.Disputes
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DisputeDetailDto(
                d.Id, d.ReferenceNumber, d.Reason.ToString(), d.Comments,
                d.Status.ToString(), d.SubmittedAt, d.ResolvedAt, d.UpdatedAt,
                new TransactionDetailDto(
                    d.Transaction.Id, d.Transaction.CustomerId, d.Transaction.TransactionDate,
                    d.Transaction.Description, d.Transaction.Amount, d.Transaction.Category.ToString(),
                    d.Transaction.Reference, d.Transaction.IsDisputed, d.Transaction.CreatedAt,
                    null,
                    new AccountInfoDto(
                        d.Transaction.AccountId,
                        d.Transaction.Account.AccountName,
                        d.Transaction.Account.AccountType.ToString(),
                        d.Transaction.Account.AccountNumber
                    )
                ),
                new CustomerInfoDto(d.Customer.Id, d.Customer.FullName, d.Customer.Email),
                new AccountInfoDto(
                    d.Transaction.AccountId,
                    d.Transaction.Account.AccountName,
                    d.Transaction.Account.AccountType.ToString(),
                    d.Transaction.Account.AccountNumber
                ),
                d.StatusHistory
                    .OrderBy(h => h.ChangedAt)
                    .Select(h => new DisputeStatusHistoryDto(
                        h.FromStatus.ToString(), h.ToStatus.ToString(),
                        h.ChangedByRole, h.Notes, h.ChangedAt
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<DisputeListItemDto>> GetAllPagedAsync(
        DisputeFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var query = ctx.Disputes.AsNoTracking();
        var filtered = ApplyFilters(query, filters);
        var total = await filtered.CountAsync(ct);

        var items = await filtered
            .OrderByDescending(d => d.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DisputeListItemDto(
                d.Id, d.ReferenceNumber,
                d.Transaction.TransactionDate, d.Transaction.Amount,
                d.Reason.ToString(), d.Status.ToString(),
                d.SubmittedAt, d.UpdatedAt,
                d.Customer.FullName,
                d.Transaction.Account.AccountType.ToString(),
                d.Transaction.Account.AccountNumber
            ))
            .ToListAsync(ct);

        return new PagedResult<DisputeListItemDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<DisputeDetailDto?> UpdateStatusAsync(
        Guid id, DisputeStatus newStatus, string notes,
        string agentId, string agentRole, CancellationToken ct)
    {
        var dispute = await ctx.Disputes
            .Include(d => d.StatusHistory)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (dispute is null) return null;

        DisputeStatusTransition.EnsureValid(dispute.Status, newStatus);

        var oldStatus = dispute.Status;
        dispute.Status = newStatus;
        dispute.UpdatedAt = DateTime.UtcNow;
        if (newStatus is DisputeStatus.Resolved or DisputeStatus.Rejected)
            dispute.ResolvedAt = DateTime.UtcNow;

        ctx.DisputeStatusHistories.Add(new DisputeStatusHistory
        {
            Id = Guid.NewGuid(),
            DisputeId = id,
            FromStatus = oldStatus,
            ToStatus = newStatus,
            ChangedByUserId = agentId,
            ChangedByRole = agentRole,
            Notes = notes,
            ChangedAt = DateTime.UtcNow
        });

        await ctx.SaveChangesAsync(ct);
        return await GetByIdAsync(id, ct);
    }

    public async Task<int> GetMaxReferenceSequenceForYearAsync(int year, CancellationToken ct)
    {
        var prefix = $"DSP-{year}-";
        var refs = await ctx.Disputes
            .AsNoTracking()
            .Where(d => d.ReferenceNumber.StartsWith(prefix))
            .Select(d => d.ReferenceNumber)
            .ToListAsync(ct);

        if (refs.Count == 0) return 0;
        return refs.Max(r => int.TryParse(r[prefix.Length..], out var n) ? n : 0);
    }

    public async Task<DisputeSummaryStatsDto> GetSummaryStatsAsync(Guid customerId, CancellationToken ct)
    {
        var counts = await ctx.Disputes
            .AsNoTracking()
            .Where(d => d.CustomerId == customerId)
            .GroupBy(d => d.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var submitted   = counts.FirstOrDefault(c => c.Status == DisputeStatus.Submitted)?.Count   ?? 0;
        var underReview = counts.FirstOrDefault(c => c.Status == DisputeStatus.UnderReview)?.Count ?? 0;
        var resolved    = counts.FirstOrDefault(c => c.Status == DisputeStatus.Resolved)?.Count    ?? 0;
        var rejected    = counts.FirstOrDefault(c => c.Status == DisputeStatus.Rejected)?.Count    ?? 0;

        return new DisputeSummaryStatsDto(submitted, underReview, resolved, rejected, submitted + underReview + resolved + rejected);
    }

    private static IQueryable<Dispute> ApplyFilters(IQueryable<Dispute> query, DisputeFilters filters)
    {
        if (filters.Status.HasValue)
            query = query.Where(d => d.Status == filters.Status.Value);
        if (filters.DateFrom.HasValue)
            query = query.Where(d => d.SubmittedAt >= filters.DateFrom.Value);
        if (filters.DateTo.HasValue)
            query = query.Where(d => d.SubmittedAt <= filters.DateTo.Value);
        if (filters.ReasonCode.HasValue)
            query = query.Where(d => d.Reason == filters.ReasonCode.Value);
        return query;
    }
}
