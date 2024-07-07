using System.Web;
using MovieInfoApi.Controllers.Abstracts;
using MovieInfoApi.Models;
using Newtonsoft.Json;

namespace MovieInfoApi.Services;

public class OmdbApi : IOmdbApi
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public OmdbApi(IConfiguration configuration, HttpClient client)
    {
        _configuration = configuration;
        _httpClient = client;
        _httpClient.BaseAddress = new Uri(_configuration["OAMBApiHost"]);
    }

    public async Task<FilmInfoOMDBResponse> MovieInfo(string title)
    {
        var uriBuilder = new UriBuilder(_httpClient.BaseAddress);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["t"] = title;
        query["apiKey"] = _configuration.GetValue<string>("OAMBApiKey");
        uriBuilder.Query = query.ToString();

        var response = await _httpClient.GetAsync(uriBuilder.ToString());
        var resultJson = await response.Content.ReadAsStringAsync();
        var movieInfo = JsonConvert.DeserializeObject<FilmInfoOMDBResponse>(resultJson);
        return movieInfo;
    }
}