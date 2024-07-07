using MongoDB.Driver;
using MovieInfoApi.Models;

namespace MovieInfoApi.Controllers.Abstracts;

public interface IMovieDbService
{
    Task<List<ClientRequestInfoResponse?>> GetFilteredRequests(FilterDefinition<ClientRequestInfo> filter);
    Task InsertMovieData(ClientRequestInfo data);
    Task<List<ClientRequestInfoResponse?>> FilterByDate(DateTime startDate, DateTime endDate);
    Task<List<DailyUsageReport>> UsagePerDay();
    Task Delete(string id);
}