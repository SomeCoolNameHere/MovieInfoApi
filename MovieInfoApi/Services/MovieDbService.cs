using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MovieInfoApi.Controllers.Abstracts;
using MovieInfoApi.Models;

namespace MovieInfoApi.Services;

public class MovieDbService : IMovieDbService
{
    private readonly IMongoCollection<ClientRequestInfo> _collection;
    private readonly IMapper _mapper;

    public MovieDbService(IMongoClient client, IOptions<RequestsInfoConfigs> schoolDatabaseSettings, IMapper mapper)
    {
        _mapper = mapper;
        var db = client.GetDatabase(schoolDatabaseSettings.Value.DatabaseName);
        _collection = db.GetCollection<ClientRequestInfo>(schoolDatabaseSettings.Value.RequestsCollectionName);
    }

    //probably could add queue here for better performance 
    public async Task InsertMovieData(ClientRequestInfo data)
    {
        await _collection.InsertOneAsync(data);
    }

    public async Task<List<ClientRequestInfoResponse?>> GetFilteredRequests(FilterDefinition<ClientRequestInfo> filter)
    {
        var result = await _collection.Find(filter).ToListAsync();
        return _mapper.Map<List<ClientRequestInfoResponse?>>(result);
    }

    public async Task<List<ClientRequestInfoResponse?>> FilterByDate(DateTime startDate, DateTime endDate)
    {
        var result = await _collection.Find(x => x.Timestamp > startDate && x.Timestamp < endDate).ToListAsync();
        return _mapper.Map<List<ClientRequestInfoResponse?>>(result);
    }

    public async Task<List<DailyUsageReport>> UsagePerDay()
    {
        var report = await _collection.Aggregate().Group(new BsonDocument
            {
                {
                    "_id", new BsonDocument
                    {
                        { "month", new BsonDocument("$month", "$timestamp") },
                        { "day", new BsonDocument("$dayOfMonth", "$timestamp") },
                        { "year", new BsonDocument("$year", "$timestamp") }
                    }
                },
                { "count", new BsonDocument("$sum", 1) }
            })
            .ToListAsync();
        return BsonSerializer.Deserialize<List<DailyUsageReport>>(report.ToJson());
    }

    public async Task Delete(string id)
    {
        await _collection.DeleteOneAsync(x => x._id == new ObjectId(id));
    }
}