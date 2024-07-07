using AutoMapper;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MovieInfoApi.Controllers.Abstracts;
using MovieInfoApi.Mapping;
using MovieInfoApi.Models;
using MovieInfoApi.Services;

namespace MovieInfoApi.Extentions;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureMovieServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<RequestsInfoConfigs>()
            .Bind(configuration.GetSection(nameof(RequestsInfoConfigs)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<IMongoClient>(_ =>
        {
            var connectionString =
                configuration
                    .GetSection("RequestsInfoConfigs:ConnectionString")?
                    .Value;
            return new MongoClient(connectionString);
        });

        services.AddHttpClient<OmdbApi>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("OAMBApiHost"));
        });

        services.AddScoped<IMovieDbService, MovieDbService>();
        services.AddScoped<IOMDBApi, OmdbApi>();
        services
            .Configure<RequestsInfoConfigs>(
                configuration.GetSection(nameof(RequestsInfoConfigs))
            );
        return services;
    }

    public static IServiceCollection AddAutomapperConfiguration(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(mc => { mc.AddProfile(new MovieServiceMapperProfile()); });
        var mapper = mapperConfiguration.CreateMapper();

        mapperConfiguration.AssertConfigurationIsValid();

        services.AddSingleton(mapper);

        return services;
    }
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieInfoApi", Version = "v1" });
            c.AddSecurityDefinition(GlobalConsts.ApiKey, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = GlobalConsts.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var key = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = GlobalConsts.ApiKey
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
            {
                { key, new List<string>() }
            };
            c.AddSecurityRequirement(requirement);
        });

        return services;
    }
    
}