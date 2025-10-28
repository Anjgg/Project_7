namespace P7CreateRestApi.Dto
{
    public class RatingDto
    {
        public int Id { get; set; }
        public string Moodys { get; set; } = string.Empty;
        public string SandP { get; set; } = string.Empty;
        public string Fitch { get; set; } = string.Empty;
        public byte? OrderNumber { get; set; }
    }
}
