using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dto
{
    public class RuleDto
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Json { get; set; } = string.Empty;

        [StringLength(200)]
        public string Template { get; set; } = string.Empty;

        [StringLength(1000)]
        public string SqlStr { get; set; } = string.Empty;

        [StringLength(500)]
        public string SqlPart { get; set; } = string.Empty;
    }
}
