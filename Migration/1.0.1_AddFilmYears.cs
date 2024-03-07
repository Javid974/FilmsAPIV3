using Models.DAO;
using Models;
using MongoDB.Driver;
using MongoDBMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration
{
    public class _1 : IMigration
    {
        public MongoDBMigrations.Version Version => "1.0.1";

        public string Name => "AddFilmYears";

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
                    Id = 3,
                    DirectorsIds = new List<int> { 4 },
                    Title = "Film 1",
                    OriginalTitle = "Film 1",
                    Overview = "Synopsis du film 1",
                    ReleaseDate = new DateTime(2023, 1, 1),
                    PosterPath = "/path/to/poster1.jpg",
                    BackdropPath = "/path/to/backdrop1.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } },
                     MightWatch = false
                },
                 new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 4,
                    DirectorsIds = new List<int> { 5 },
                    Title = "Film 2",
                    OriginalTitle = "Film 2",
                    Overview = "Synopsis du film 2",
                    ReleaseDate = new DateTime(2023, 2, 2),
                    PosterPath = "/path/to/poster2.jpg",
                    BackdropPath = "/path/to/backdrop2.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } },
                     MightWatch = false
                },
                 new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 5,
                    DirectorsIds = new List<int> { 6 },
                    Title = "Film 3",
                    OriginalTitle = "Film 3",
                    Overview = "Synopsis du film 3",
                    ReleaseDate = new DateTime(2023, 3, 3),
                    PosterPath = "/path/to/poster3.jpg",
                    BackdropPath = "/path/to/backdrop3.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" } },
                     MightWatch = false
                }
            };

            database.GetCollection<MovieDAO>(collectionNameMovie).InsertMany(movies);

            List<Director> directors = new List<Director>
            {
                new Director
                {
                    Id = 4,
                    Name = "Nom du réalisateur 4"
                },
                new Director
                {
                    Id = 5,
                    Name = "Nom du réalisateur 5"
                },
                new Director
                {
                    Id = 6,
                    Name = "Nom du réalisateur 6"
                }
            };

            database.GetCollection<Director>(collectionNameDirectors).InsertMany(directors);
        }
    }
}
