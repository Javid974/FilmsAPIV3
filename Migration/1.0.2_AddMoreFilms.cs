using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration
{
    using Models.DAO;
    using Models;
    using MongoDB.Driver;
    using MongoDBMigrations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class _2 : IMigration
    {
        public MongoDBMigrations.Version Version => "1.0.2";

        public string Name => "AddMoreFilms";

        public void Down(IMongoDatabase database)
        {

        }

        public void Up(IMongoDatabase database)
        {
            var collectionNameMovie = "Movies";
            var collectionNameDirectors = "Directors";
            List<MovieDAO> movies = new List<MovieDAO>
            {
                new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 6,
                    DirectorsIds = new List<int> { 7, 8 },
                    Title = "Film 4",
                    OriginalTitle = "Film 4",
                    Overview = "Synopsis du film 4",
                    ReleaseDate = new DateTime(2022, 1, 1),
                    PosterPath = "/path/to/poster4.jpg",
                    BackdropPath = "/path/to/backdrop4.jpg",
                    Support = Support.None,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } },
                    MightWatch = false
                },
                new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 7,
                    DirectorsIds = new List<int> { 9 },
                    Title = "Film 5",
                    OriginalTitle = "Film 5",
                    Overview = "Synopsis du film 5",
                    ReleaseDate = new DateTime(2022, 2, 2),
                    PosterPath = "/path/to/poster5.jpg",
                    BackdropPath = "/path/to/backdrop5.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 18, Name = "Drama" },
                        new Genre { Id = 35, Name = "Comedy" },
                        new Genre { Id = 53, Name = "Thriller" },
                    },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } },
                    MightWatch = false
                },
                new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 12,
                    DirectorsIds = new List<int> { 9 },
                    Title = "Film 12",
                    OriginalTitle = "Film 12",
                    Overview = "Synopsis du film 12",
                    ReleaseDate = new DateTime(2023, 2, 2),
                    PosterPath = "/path/to/poster12.jpg",
                    BackdropPath = "/path/to/backdrop12.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 18, Name = "Drama" },
                        new Genre { Id = 35, Name = "Comedy" },
                        new Genre { Id = 53, Name = "Thriller" },
                    },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "CA", English_name = "Canada", Native_name ="Canada", Region = "Americas"  } },
                    MightWatch = false
                },
                   new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 13,
                    DirectorsIds = new List<int> { 9 },
                    Title = "Film 13",
                    OriginalTitle = "Film 13",
                    Overview = "Synopsis du film 13",
                    ReleaseDate = new DateTime(2023, 2, 2),
                    PosterPath = "/path/to/poster13.jpg",
                    BackdropPath = "/path/to/backdrop13.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre>
                    {
                        new Genre { Id = 18, Name = "Drama" },
                        new Genre { Id = 35, Name = "Comedy" },
                        new Genre { Id = 53, Name = "Thriller" },
                    },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "FR", English_name = "France", Native_name = "France", Region = "Europe"  } },
                    MightWatch = false
                },
                new MightWatchMovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 8,
                    DirectorsIds = new List<int> { 10 },
                    Title = "Film 6",
                    OriginalTitle = "Film 6",
                    Overview = "Synopsis du film 6",
                    ReleaseDate = new DateTime(2022, 3, 3),
                    PosterPath = "/path/to/poster6.jpg",
                    BackdropPath = "/path/to/backdrop6.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country>
                    {
                        new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America",  Region = "Americas"  },
                        new Country { Iso_3166_1 = "FR", English_name = "United States of America", Native_name = "France",  Region = "Europe"  },
                    },
                    MightWatch = true
                },
                 new MightWatchMovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 9,
                    DirectorsIds = new List<int> { 10 },
                    Title = "Film 7",
                    OriginalTitle = "Film 7",
                    Overview = "Synopsis du film 7",
                    ReleaseDate = new DateTime(2022, 3, 3),
                    PosterPath = "/path/to/poster7.jpg",
                    BackdropPath = "/path/to/backdrop7.jpg",
                    Support = Support.BluRay,
                    ImdbNote = 3.4,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country>
                    {
                        new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America",  Region = "Americas"  },
                        new Country { Iso_3166_1 = "FR", English_name = "United States of America", Native_name = "France",  Region = "Europe"  },
                    },
                    MightWatch = true
                },
                 new MightWatchMovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 10,
                    DirectorsIds = new List<int> { 10 },
                    Title = "Film 8",
                    OriginalTitle = "Film 8",
                    Overview = "Synopsis du film 8",
                    ReleaseDate = new DateTime(2022, 3, 3),
                    PosterPath = "/path/to/poster8.jpg",
                    BackdropPath = "/path/to/backdrop8.jpg",
                    Support = Support.BluRay,
                    SenscritiqueNote = 4.0,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country>
                    {
                        new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region= "Americas" },
                        new Country { Iso_3166_1 = "FR", English_name = "United States of America", Native_name = "France", Region= "Europe" },
                    },
                    MightWatch = true
                },
                      new MightWatchMovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 11,
                    DirectorsIds = new List<int> { 10 },
                    Title = "Film 9",
                    OriginalTitle = "Film 9",
                    Overview = "Synopsis du film 9",
                    ReleaseDate = new DateTime(2022, 3, 3),
                    PosterPath = "/path/to/poster8.jpg",
                    BackdropPath = "/path/to/backdrop8.jpg",
                    Support = Support.BluRay,
                    SenscritiqueNote = 8.2,
                    ImdbNote = 7.0,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country>
                    {
                        new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } ,
                        new Country { Iso_3166_1 = "FR", English_name = "France", Native_name = "France", Region = "Europe" },
                    },
                    MightWatch = true
                },

            };
            database.GetCollection<MovieDAO>(collectionNameMovie).InsertMany(movies);

            List<Director> directors = new List<Director>
            {
                new Director
                {
                    Id = 7,
                    Name = "Nom du réalisateur 7"
                },
                new Director
                {
                    Id = 8,
                    Name = "Nom du réalisateur 8"
                },
                new Director
                {
                    Id = 9,
                    Name = "Nom du réalisateur 9"
                },
                new Director
                {
                    Id = 10,
                    Name = "Nom du réalisateur 10"
                },
            };

            database.GetCollection<Director>(collectionNameDirectors).InsertMany(directors);

        }
    }
}
