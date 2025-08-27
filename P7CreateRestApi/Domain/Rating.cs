namespace Dot.Net.WebApi.Domain
{
    public class Rating
    {
        // TODO: Map columns in data table RATING with corresponding fields

        public int Id { get; set; }
        public string MoodysRating { get; set; } = string.Empty;
        public string SandPRating { get; set; } = string.Empty;
        public string FitchRating { get; set; } = string.Empty;
        public byte? OrderNumber { get; set; }

    }
}