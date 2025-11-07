using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public class BidDto
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Account { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Type { get; set; } = string.Empty;

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "BidQuantity must be positive")]
        public double? BidQuantity { get; set; }

        [Required, Range(0.0001, double.MaxValue, ErrorMessage = "AskQuantity must be positive")]
        public double? AskQuantity { get; set; }

        [Required, StringLength(50)]
        public string Benchmark { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        public DateTime? BidDate { get; set; }

        [StringLength(500)]
        public string Commentary { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string BidSecurity { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string BidStatus { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Trader { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Book { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string CreationName { get; set; } = string.Empty;

        [Required, DataType(DataType.DateTime)]
        public DateTime? CreationDate { get; set; }

        [Required, StringLength(50)]
        public string RevisionName { get; set; } = string.Empty;

        [Required, DataType(DataType.DateTime)]
        public DateTime? RevisionDate { get; set; }

        [Required, StringLength(50)]
        public string DealName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string DealType { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string SourceListId { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string Side { get; set; } = string.Empty;
    }
}
