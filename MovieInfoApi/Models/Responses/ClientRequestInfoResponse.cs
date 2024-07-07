namespace MovieInfoApi.Models;

public class ClientRequestInfoResponse
{
    public string Id { get; set; }
    public string SearchToken { get; set; }
    public string imdbId { get; set; }
    public int ProcessingTimeMs { get; set; }
    public DateTime Timestamp { get; set; }
    public string IpAddress { get; set; }
}