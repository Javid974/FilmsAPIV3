
using MongoDB.Bson;
using System.Collections.Generic;
using System.Data.Entity;
using Models.DAO;
using MongoDB.Driver;
//using MongoDB.Migrations;
using Models;
using MongoDBMigrations;
using MongoDB.Driver.Core.Configuration;

namespace FilmsDatabase
{
    public class InitialMigration : IMigration
    {

        public MongoDBMigrations.Version Version => "1.0.0";

        public string Name => "InitialMigration";


        public void Down(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }


        public void Up(IMongoDatabase database)
        {

            var collectionNameMovie = "Movies";
            var collectionNameDirectors = "Directors";
            database.DropCollection(collectionNameMovie);

            database.CreateCollection(collectionNameMovie);

            List<MovieDAO> movies = new List<MovieDAO>
            {
                new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 1,
                    DirectorsIds = new List<int> { 1, 2 },
                    Title = "The Shawshank Redemption",
                    OriginalTitle = "The Shawshank Redemption",
                    Overview = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    PosterPath = "/9O7gLzmreU0nGkIB6K3BsJbzvNv.jpg",
                    BackdropPath = "/j9XKiZrVeViAixVRzCta7h1VU9W.jpg",
                    Support = Support.BluRay,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America",  Region = "Americas" } },
                    MightWatch = false
                },
                new MovieDAO
                {
                    Uuid = Guid.NewGuid(),
                    Id = 2,
                    DirectorsIds = new List<int> { 3 },
                    Title = "The Godfather",
                    OriginalTitle = "The Godfather",
                    Overview = "An organized crime dynasty's aging patriarch transfers control of his clandestine empire to his reluctant son.",
                    ReleaseDate = new DateTime(1972, 03, 24),
                    PosterPath = "/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
                    BackdropPath = "/jMBPQdZ8P7udPRw0z0HAvYs4oZS.jpg",
                    Support = Support.Netflix,
                    Genres = new List<Genre> { new Genre { Id = 18, Name = "Drama" }, new Genre { Id = 80, Name = "Crime" } },
                    Countries = new List<Country> { new Country { Iso_3166_1 = "US", English_name = "United States of America", Native_name = "United States of America", Region = "Americas" }, new Country { Iso_3166_1 = "IT", English_name = "Italy", Native_name = "Italia", Region = "Europe" } },
                     MightWatch = false
                }
            };

            database.GetCollection<MovieDAO>(collectionNameMovie).InsertMany(movies);

            database.DropCollection(collectionNameDirectors);
            List<Director> directors = new List<Director>
            {
                new Director
                {
                    Id = 1,
                    Name = "Nom du réalisateur 1"
                },

                new Director
                {
                    Id = 2,
                    Name = "Nom du réalisateur 2"
                }
            };

            database.GetCollection<Director>(collectionNameDirectors).InsertMany(directors);

        }
    }
}





