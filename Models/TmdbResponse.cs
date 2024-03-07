using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models
{
    public class TMDbResponse
    {
        public TMDbResponse()
        {
            Page = 0;
            Results = new List<MovieResult>();
        }
        public int Page { get; set; }
        public List<MovieResult> Results { get; set; }
        // Add other properties if needed
    }

    public class MovieResult
    {
        public MovieResult(string backdropPath, string posterPath, int id, string originalLanguage, string originalTitle, List<int> genreIds, string title, string overview, string releaseDate)
        {

            PosterPath = posterPath;
            Id = id;
            OriginalLanguage = originalLanguage;
            OriginalTitle = originalTitle;
            GenreIds = genreIds;
            Title = title;
            Overview = overview;
            BackdropPath = backdropPath;
            ReleaseDate = releaseDate;
        }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
    public class Person
    {
        public Person()
        {
            ProfilePath = string.Empty;
            Name = string.Empty;
            OriginalName = string.Empty;
            KnownForDepartment = string.Empty;
        }   
        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("known_for_department")]
        public string KnownForDepartment { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
    }

    public class Cast : Person
    {
        public Cast()
        {
            Character = string.Empty;
            CreditId = string.Empty;
        }

        [JsonProperty("cast_id")]
        public int castId { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }

    public class Crew : Person
    {
        public Crew()
        {
            Department = string.Empty;
            Job = string.Empty;
            CreditId = string.Empty;
        }

        [JsonProperty("credit_id")]
        public string CreditId { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }
    }

    public class MovieCredits
    {
        public MovieCredits()
        {
            Cast = new List<Cast>();
            Crew = new List<Crew>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("cast")]
        public List<Cast> Cast { get; set; }

        [JsonProperty("crew")]
        public List<Crew> Crew { get; set; }
    }


    public class MovieDetails
    {
        public MovieDetails()
        {
            Genres = new List<Genre>();
            ProductionCountries = new List<ProductionCountry>();
            Title = string.Empty;
            OriginalTitle = string.Empty;
            ReleaseDate = string.Empty;
            Overview = string.Empty;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        public List<Genre> Genres { get; set; }
        [JsonProperty("production_countries")]

        public List<ProductionCountry> ProductionCountries { get; set; }
    }

    public class ProductionCountry
    {
        public ProductionCountry(string iso_3166_1, string name)
        {
            Iso_3166_1 = iso_3166_1;
            Name = name;
        }
        public string Iso_3166_1 { get; set; }
        public string Name { get; set; }
    }



}
