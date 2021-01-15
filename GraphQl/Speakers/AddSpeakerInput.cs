namespace ConferencePlanner.GraphQl
{
    public record AddSpeakerInput(
        string Name,
        string? Bio,
        string? WebSite);
}