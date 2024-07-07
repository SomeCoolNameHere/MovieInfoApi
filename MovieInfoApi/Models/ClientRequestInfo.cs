using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieInfoApi.Models;

public class ClientRequestInfo
{
    public ClientRequestInfo()
    {
        _id = ObjectId.GenerateNewId();
    }

    [BsonId]
    public BsonObjectId _id { get; set; }

    [BsonElement("search_token")]
    public string SearchToken { get; set; }

    [BsonElement("imdbID")]
    public string imdbId { get; set; }

    [BsonElement("processing_time_ms")]
    public int ProcessingTimeMs { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("ip_address")]
    public string IpAddress { get; set; }
}