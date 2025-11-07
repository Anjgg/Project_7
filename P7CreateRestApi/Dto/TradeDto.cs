using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public class TradeDto
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Account { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string AccountType { get; set; } = string.Empty;

        [Range(0.0001, double.MaxValue, ErrorMessage = "BuyQuantity must be positive")]
        public double? BuyQuantity { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "SellQuantity must be positive")]
        public double? SellQuantity { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "BuyPrice must be positive")]
        public double? BuyPrice { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "SellPrice must be positive")]
        public double? SellPrice { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TradeDate { get; set; }

        [Required, StringLength(50)]
        public string TradeSecurity { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string TradeStatus { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Trader { get; set; } = string.Empty;

        [StringLength(50)]
        public string Benchmark { get; set; } = string.Empty;

        [StringLength(50)]
        public string Book { get; set; } = string.Empty;

        [StringLength(50)]
        public string CreationName { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime? CreationDate { get; set; }

        [StringLength(50)]
        public string RevisionName { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime? RevisionDate { get; set; }

        [StringLength(50)]
        public string DealName { get; set; } = string.Empty;

        [StringLength(50)]
        public string DealType { get; set; } = string.Empty;

        [StringLength(50)]
        public string SourceListId { get; set; } = string.Empty;

        [StringLength(10)]
        public string Side { get; set; } = string.Empty;
    }
}
