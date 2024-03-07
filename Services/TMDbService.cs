using Amazon.Runtime;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Options;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TMDbService
    {
        private readonly HttpClient _client;
        private readonly ApiSettings _settings;

        public TMDbService()
        {
            _client = new HttpClient();
            _settings = new ApiSettings();
        }

        public TMDbService(IHttpClientFactory client, IOptions<ApiSettings> settings)
        {
            _client = client.CreateClient();
            _settings = settings.Value;
        }


        public async Task<string> GetRegion(string countryCode)
        {
            var response = await _client.GetAsync($"{_settings.RestCountriesBaseUrl}/{countryCode}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countryDetails = JsonConvert.DeserializeObject<RestCountry>(content);
            if (countryDetails != null)
            {
                return countryDetails.Region;
            }
            throw new NotFoundException($"Region with code '{countryCode}' not found");
        }

        public async Task<IEnumerable<Country>> GetTranslatedCountries(MovieDetails movieDetails, List<Country> allCountriesFromApi)
        {
            var countriesWithRegions = new List<Country>();
            foreach (var productionCountry in movieDetails.ProductionCountries)
            {
                var correspondingCountry = allCountriesFromApi.FirstOrDefault(apiCountry => apiCountry.Iso_3166_1 == productionCountry.Iso_3166_1);

                if (correspondingCountry == null) { continue; }
                var region = await GetRegion(productionCountry.Iso_3166_1);

                countriesWithRegions.Add(new Country
                {
                    Iso_3166_1 = productionCountry.Iso_3166_1,
                    English_name = productionCountry.Name,
                    Native_name = correspondingCountry.Native_name,
                    Region = TraduceRegion(region)
                });
            }

            return countriesWithRegions;
        }

        public string TraduceRegion(string region)
        {
            Region regionEnum;
            switch (region)
            {
                case "Europe":
                    regionEnum = Region.Europe;
                    break;
                case "Asia":
                    regionEnum = Region.Asie;
                    break;
                case "Africa":
                    regionEnum = Region.Afrique;
                    break;
                case "Americas":
                    regionEnum = Region.Amerique;
                    break;
                case "Amérique":
                    regionEnum = Region.Amerique;
                    break;
                case "Oceania":
                    regionEnum = Region.Oceanie;
                    break;
                default:
                    regionEnum = Region.Autre;
                    break;
            }

            return regionEnum.ToString();
        }

        private async Task AddDirectorsToMovie(Movie inputMovie, Movie tmdbMovie)
        {
            var directors = await GetDirectors(tmdbMovie.Id);
            if (directors != null)
            {
                foreach (var director in directors)
                {
                    var existingDirector = inputMovie.Directors.FirstOrDefault(d => d.Name.ToLower() == director.Name.ToLower());
                    if (existingDirector != null)
                    {
                        tmdbMovie.Directors.Add(new Director() { Id = director.Id, Name = director.Name });
                    }
                }
            }
        }

        public async Task<Movie> GetMovie(Movie movie)
        {
            var title = movie.Title;
            List<Movie>? movies = await SearchMovies(title);
            if (movies == null || movies.Count == 0)
            {
                throw new NotFoundException($"Movie with title '{title}' not found"); ;
            }

            foreach (var tmdbMovie in movies)
            {
                await AddDirectorsToMovie(movie, tmdbMovie);
                if (tmdbMovie.Directors.Count > 0)
                {
                    return tmdbMovie;
                }

            }
            throw new NotFoundException($"Director {string.Join(", ", movie.Directors.Select(x => x.Name))} for the movie with title '{title}' not found");
        }


        private async Task<List<Movie>?> SearchMovies(string title)
        {
            var response = await _client.GetAsync($"{_settings.SearchBaseUrl}?api_key={_settings.ApiKey}&query={title}&language=fr-FR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tmdbResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TMDbResponse>(content);
            var movies = tmdbResponse?.Results.Where(r => r.Title.ToLower() == title.ToLower().Trim()).Select(MapToMovie).ToList() ?? new List<Movie>();

            if (!movies.Any() && tmdbResponse?.Results.Any() == true)
            {
                movies.Add(MapToMovie(tmdbResponse.Results[0]));
            }

            return movies;
        }

        public async Task<MovieDetails> GetMovieDetails(int movieId)
        {
            var response = await _client.GetAsync($"{_settings.MovieBaseUrl}/{movieId}?api_key={_settings.ApiKey}&language=fr-FR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(content);
            if (movieDetails == null)
            {
                throw new NotFoundException($"Movie with id '{movieId}' not found");
            }
            return movieDetails;
        }

        public async Task<List<Country>> GetMovieProductionCountries()
        {
            var response = await _client.GetAsync($"{_settings.BaseUrl}/configuration/countries?api_key={_settings.ApiKey}&language=fr-FR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<Country>>(content);
            if (countries == null)
            {
                throw new NotFoundException($"Countries not found");
            }
            return countries;
        }


        public async Task<List<Crew>> GetDirectors(int movieId)
        {
            var response = await _client.GetAsync($"{_settings.MovieBaseUrl}/{movieId}/credits?api_key={_settings.ApiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            MovieCredits? root = JsonConvert.DeserializeObject<MovieCredits>(content);
            if (root == null)
            {
                throw new NotFoundException($"Directors for movie with id '{movieId}' not found");
            }
            List<Crew> directors = root.Crew.Where(c => c.Job == "Director").ToList();


            if (directors != null && directors.Count > 0)
            {
                return directors;
            }
            else
            {
                return new List<Crew>();
            }
        }

        private Movie MapToMovie(MovieResult tmdbResult)
        {
            DateTime.TryParse(tmdbResult.ReleaseDate, out DateTime parsedDate);
            return new Movie
            {
                Uuid = Guid.NewGuid(),
                Title = tmdbResult.Title,
                OriginalTitle = tmdbResult.OriginalTitle,
                ReleaseDate = parsedDate,
                Id = tmdbResult.Id,
                Overview = tmdbResult.Overview,
                PosterPath = tmdbResult.PosterPath,
                BackdropPath = tmdbResult.BackdropPath,
                MightWatch = false,
            };
        }

    }

}
