namespace P7CreateRestApi.Models
{
    public class BidDto
    {
        public int Id { get; set; }
        public required string Account { get; set; }
        public required string Type { get; set; }
        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public string Benchmark { get; set; } = string.Empty;
        public DateTime? BidDate { get; set; }
        public string Commentary { get; set; } = string.Empty;
        public string BidSecurity { get; set; } = string.Empty;
        public string BidStatus { get; set; } = string.Empty;
        public string Trader { get; set; } = string.Empty;
        public string Book { get; set; } = string.Empty;
        public string CreationName { get; set; } = string.Empty;
        public DateTime? CreationDate { get; set; }
        public string RevisionName { get; set; } = string.Empty;
        public DateTime? RevisionDate { get; set; }
        public string DealName { get; set; } = string.Empty;
        public string DealType { get; set; } = string.Empty;
        public string SourceListId { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
    }
}
