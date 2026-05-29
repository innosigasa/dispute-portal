namespace DisputePortal.Application.Domain.Models
{
    public abstract class EnumModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
    }
}
