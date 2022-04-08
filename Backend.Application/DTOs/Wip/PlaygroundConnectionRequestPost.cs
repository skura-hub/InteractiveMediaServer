namespace Backend.Application.DTOs.Wip
{
    public class PlaygroundConnectionRequestPost
    {
        public int ArtworkId { get; set; }
        public int NodeStartingId { get; set; }
        public int NodeEndingId { get; set; }
        public string ShortName { get; set; }
        public bool IsDefault { get; set; }

    }
}
