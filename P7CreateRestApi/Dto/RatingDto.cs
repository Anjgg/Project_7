using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public class RatingDto
    {
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string Moodys { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string SandP { get; set; } = string.Empty;

        [Required, StringLength(10)]
        public string Fitch { get; set; } = string.Empty;

        [Range(1, 255, ErrorMessage = "OrderNumber must be between 1 and 255")]
        public byte? OrderNumber { get; set; }
    }
}
