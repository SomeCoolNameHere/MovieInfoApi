using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MovieInfoApi.Controllers.Abstracts;
using MovieInfoApi.Models;

namespace MovieInfoApi.Controllers;

[AdminAuth]
[ApiController]
[Route("[controller]/[action]")]
public class AdminQueriesController
{
    private readonly IMovieDbService _movieDbService;

    public AdminQueriesController(IMovieDbService movieDbService)
    {
        _movieDbService = movieDbService;
    }

    /// <summary>
    ///     return all requests that have been requested by clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ClientRequestInfoResponse>> AllRequestEntries()
    {
        // possible performance issue because could be a lot of queries
        // need to use pagination or other practice to improve performance in future
        return await _movieDbService.GetFilteredRequests(Builders<ClientRequestInfo>.Filter.Empty);
    }

    /// <summary>
    ///     get one request of client data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ClientRequestInfoResponse> RequestEntryById(string id)
    {
        return (await _movieDbService.GetFilteredRequests(
            Builders<ClientRequestInfo>.Filter.Eq("_id", new BsonObjectId(id)))).FirstOrDefault();
    }

    /// <summary>
    ///     filter requests by date (between first date and last date)
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ClientRequestInfoResponse>> FilterByDate(DateTime startDate, DateTime endDate)
    {
        return await _movieDbService.FilterByDate(startDate, endDate);
    }

    /// <summary>
    ///     return a daily report of using the movie info method by clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<DailyUsageReport>> UsagePerDay()
    {
        return await _movieDbService.UsagePerDay();
    }

    /// <summary>
    ///     delete concrete request record of the client
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete]
    public async Task DeleteRequestRecord(string id)
    {
        await _movieDbService.Delete(id);
    }
}