namespace DisputePortal.Application.Domain.Entities;

public class TransactionCategoryLookup
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
