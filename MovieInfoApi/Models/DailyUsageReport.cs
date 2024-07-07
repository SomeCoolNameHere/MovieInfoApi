using MongoDB.Bson.Serialization.Attributes;

namespace MovieInfoApi.Models;

public class DailyUsageReport
{
    [BsonElement("_id")]
    public DateInfo Date { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }
}

public class DateInfo
{
    [BsonElement("day")]
    public int Day { get; set; }

    [BsonElement("month")]
    public int Month { get; set; }

    [BsonElement("year")]
    public int Year { get; set; }
}