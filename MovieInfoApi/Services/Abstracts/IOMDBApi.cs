using MovieInfoApi.Models;

namespace MovieInfoApi.Controllers.Abstracts;

public interface IOmdbApi
{
    public Task<FilmInfoOMDBResponse> MovieInfo(string title);
}