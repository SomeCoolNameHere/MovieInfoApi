using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieInfoApi.Controllers.Abstracts;
using MovieInfoApi.Models;

namespace MovieInfoApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SearchController : Controller
{
    private readonly IMovieDbService _movieDbService;
    private readonly IOMDBApi _omdbApi;

    public SearchController(IOMDBApi omdbApi, IMovieDbService movieDbService)
    {
        _omdbApi = omdbApi;
        _movieDbService = movieDbService;
    }

    [HttpGet]
    public async Task<FilmInfoOMDBResponse> FilmInfo(string title)
    {
            var timer = new Stopwatch();
            timer.Start();
            var movie = await _omdbApi.MovieInfo(title);
            timer.Stop();
            _movieDbService.InsertMovieData(new ClientRequestInfo
            {
                imdbId = movie.ImdbID,
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                ProcessingTimeMs = timer.Elapsed.Microseconds,
                Timestamp = DateTime.Now,
                SearchToken = movie.Title
            });
            return movie;
    }
}