namespace Backend.Application.DTOs.Wip
{
    public class PlaygroundNodeRequestPatch
    {
        public int Id { get; set; }
        public int ArtworkId { get; set; }
        public string? Name { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? MediaId { get; set; }

    }
}
