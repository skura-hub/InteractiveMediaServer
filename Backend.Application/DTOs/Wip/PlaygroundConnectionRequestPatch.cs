namespace Backend.Application.DTOs.Wip
{
    public class PlaygroundConnectionRequestPatch
    {
        public int Id { get; set; }
        public int ArtworkId { get; set; }
        public string? ShortName { get; set; }
        public string? LongName { get; set; }
        public bool? IsDefault { get; set; }

    }
}
