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

    [HttpGet]
    public async Task<List<ClientRequestInfoResponse>> AllRequestEntries()
    {
        return await _movieDbService.GetFilteredRequests(Builders<ClientRequestInfo>.Filter.Empty);
    }

    [HttpGet]
    public async Task<ClientRequestInfoResponse> RequestEntryById(string id)
    {
        return (await _movieDbService.GetFilteredRequests(
            Builders<ClientRequestInfo>.Filter.Eq("_id", new BsonObjectId(id)))).FirstOrDefault();
    }

    [HttpGet]
    public async Task<List<ClientRequestInfoResponse>> FilterByDate(DateTime startDate, DateTime endDate)
    {
        return await _movieDbService.FilterByDate(startDate, endDate);
    }

    [HttpGet]
    public async Task<List<DailyUsageReport>> UsagePerDay()
    {
        return await _movieDbService.UsagePerDay();
    }

    [HttpDelete]
    public async Task DeleteRequestRecord(string id)
    {
        await _movieDbService.Delete(id);
    }
}