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
    private readonly IOmdbApi _omdbApi;

    public SearchController(IOmdbApi omdbApi, IMovieDbService movieDbService)
    {
        _omdbApi = omdbApi;
        _movieDbService = movieDbService;
    }

    /// <summary>
    ///     Returns the information about the movie and put request data inside db
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<FilmInfoOMDBResponse> FilmInfo(string title)
    {
        //instead of rest should use outer gateway service if requests are too much,
        //that connects to this functionality through queue
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