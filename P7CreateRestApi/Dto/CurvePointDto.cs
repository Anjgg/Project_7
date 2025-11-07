using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public class CurvePointDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AsOfDate { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "Term must be positive")]
        public double? Term { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "CurvePointValue must be positive")]
        public double? CurvePointValue { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreationDate { get; set; }
    }
}
