using MovieInfoApi.Models;

namespace MovieInfoApi.Controllers.Abstracts;

public interface IOMDBApi
{
    public Task<FilmInfoOMDBResponse> MovieInfo(string title);
}