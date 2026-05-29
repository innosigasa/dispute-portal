namespace DisputePortal.Application.Feature.Lookup.Results;

public record DisputeReasonDto
{
    public DisputeReasonDto(int Id, string Code, string Label)
    {
        this.Id = Id;
        this.Code = Code;
        this.Label = Label;
    }

    public int Id { get; init; }
    public string Code { get; init; }
    public string Label { get; init; }

    public void Deconstruct(out int Id, out string Code, out string Label)
    {
        Id = this.Id;
        Code = this.Code;
        Label = this.Label;
    }
}

public record TransactionCategoryDto
{
    public TransactionCategoryDto(int Id, string Code, string Label)
    {
        this.Id = Id;
        this.Code = Code;
        this.Label = Label;
    }

    public int Id { get; init; }
    public string Code { get; init; }
    public string Label { get; init; }

    public void Deconstruct(out int Id, out string Code, out string Label)
    {
        Id = this.Id;
        Code = this.Code;
        Label = this.Label;
    }
}
