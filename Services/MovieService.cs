using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models;
using Models.DAO;
using Models.ViewModel;
using Newtonsoft.Json;
using Repository;
using Repository.Interface;
using Services.Exceptions;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly ExcelFileParser _excelFileParser;
        private readonly ILogger<MovieService> _logger;
        private readonly TMDbService _tmdbService;
        public MovieService(IMovieRepository movieRepository, IDirectorRepository directorRepository, ExcelFileParser excelFileParser, ILogger<MovieService> logger, TMDbService tMDbService)
        {
            _movieRepository = movieRepository;
            _directorRepository = directorRepository;
            _excelFileParser = excelFileParser;
            _logger = logger;
            _tmdbService = tMDbService;
        }

        public async Task Create(Movie movie)
        {
            try
            {
                if (movie.Id != 0)
                {
                    var existingMovie = await _movieRepository.GetById(movie.Id);
                    if (existingMovie != null)
                    {
                        throw new ConflictException($"Le film '{existingMovie.Title}' existe déjà.");
                    }

                }

                bool mightWatch = movie.MightWatch.GetValueOrDefault(false);
                if (mightWatch)
                {
                    await _movieRepository.Create(MapToMightWatchMovieDAO(movie));
                }
                else
                {
                    var movieDAO = await _movieRepository.Create(MapToMovieDAO(movie));
                    movie.Id = movieDAO.Id;

                }

                await ManageMovieDirectors(movie);
            }
            catch (Exception ex)
            {
                // Consider logging the entire exception here, not just the message
                throw new InternalServerErrorException(ex.Message);
            }
        }

        private MovieDAO MapToMovieDAO(Movie movie)
        {
            var movieDao = movie.Map<Movie, MovieDAO>();
            movieDao.DirectorsIds = movie.Directors.Select(d => d.Id).ToList();
            return movieDao;
        }

        private MightWatchMovieDAO MapToMightWatchMovieDAO(Movie movie)
        {
            var mightWatchMovieDAO = movie.Map<Movie, MightWatchMovieDAO>();
            mightWatchMovieDAO.DirectorsIds = movie.Directors.Select(d => d.Id).ToList();
            return mightWatchMovieDAO;
        }

        private async Task ManageMovieDirectors(Movie movie)
        {
            foreach (var director in movie.Directors)
            {
                var existingDirector = await GetExisting(director);
                //si le director n'existe pas on le crée, sinon on met à jour son id si nécessaire
                if (existingDirector == null)
                {
                    await Save(director);
                }
                else if (existingDirector.Id != 0 && director.Id == 0)
                {
                    director.Id = existingDirector.Id;
                    await Update(movie);

                }
            }
        }

        private async Task<Director?> GetExisting(Director director)
        {
            if (director.Id == 0)
            {
                return await _directorRepository.GetByName(director.Name);
            }
            else
            {
                var directorById = await _directorRepository.GetById(director.Id);
                if (directorById != null)
                {
                    return directorById;
                }
                else
                {
                    return await _directorRepository.GetByName(director.Name);
                }

            }
        }

        private async Task<Director> Save(Director director)
        {
            return await _directorRepository.Save(director);
        }

        public async Task<MovieDAO> Update(Movie movie)
        {
            try
            {
                // Map the incoming movie to MovieDAO
                var movieDaoToUpdate = movie.Map<Movie, MovieDAO>();

                // Update the directors information
                movieDaoToUpdate.DirectorsIds = movie.Directors.Select(d => d.Id).ToList();

                foreach (var director in movie.Directors)
                {
                    var existingDirector = await _directorRepository.GetByName(director.Name);
                    if (existingDirector != null)
                    {
                        if (existingDirector.Id != director.Id)
                        {
                            //si on a changer le du realisateur pour un autre, on update le id
                            director.Id = existingDirector.Id;
                            await _directorRepository.Update(director);
                        }

                    }
                    else
                    {
                        //si on a changer le nom du realisateur, on update
                        await _directorRepository.Update(director);
                    }
                    //dans les autre cas, rien a changer, on update pas

                }

                // Save the changes
                return await _movieRepository.Update(movieDaoToUpdate);
            }
            catch (Exception ex)
            {
                // If there is any error, throw an internal server error exception
                throw new InternalServerErrorException(ex.Message);
            }
        }

        public async Task Delete(Guid uuid)
        {
            try
            {
                await _movieRepository.Delete(uuid);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message);
            }
        }

        public async Task<List<Movie>> GetByViewingYearsDate(int yearDate)
        {
            try
            {
                return await _movieRepository.GetByViewingYearsDate(yearDate);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException(ex.Message);
            }

        }

        public async Task<Movie> Get(Guid id)
        {
            return await _movieRepository.GetById(id);
        }

        public async Task<FileContentResult> Download()
        {
           var movies = await _movieRepository.GetAll();
           
            var jsonString = JsonConvert.SerializeObject(movies);

            var content = System.Text.Encoding.UTF8.GetBytes(jsonString);
            var contentType = "application/json";
            var fileName = "movies.json";

            return new FileContentResult(content, contentType) { FileDownloadName = fileName };
        }


        public async Task<string> Upload(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var parsedMovies = _excelFileParser.ParseExcelFile(stream);
                    var allCountriesFromApi = await _tmdbService.GetMovieProductionCountries();
                    foreach (var parsedMovie in parsedMovies)
                    {
                        var movie = await _tmdbService.GetMovie(parsedMovie);

                        // Now you can use movieDetails to fill in the rest of the film's properties
                        movie.Id = movie.Id;
                        movie.Uuid = movie.Uuid;
                        movie.Title = movie.Title;
                        movie.OriginalTitle = movie.OriginalTitle;
                        movie.Overview = movie.Overview;
                        movie.ReleaseDate = movie.ReleaseDate.ToUniversalTime();
                        movie.BackdropPath = movie.BackdropPath;
                        movie.PosterPath = movie.PosterPath;
                        movie.Support = parsedMovie.Support;
                        movie.ViewingDate = parsedMovie.ViewingDate;
                        movie.ViewingYear = parsedMovie.ViewingYear;
                        var directors = movie.Directors.Select(d => new Director { Id = d.Id, Name = d.Name }).ToList();
                        movie.Directors = directors;
                        movie.MightWatch = false;
                        var movieDetails = await _tmdbService.GetMovieDetails(movie.Id);
                        movie.Genres = movieDetails.Genres;
                        var countries = await _tmdbService.GetTranslatedCountries(movieDetails, allCountriesFromApi);
                        movie.Countries = countries.ToList();

                        await Create(movie);

                        // And so on...
                    }

                    return "File uploaded successfully";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading file.");
                throw;
            }
        }

        public async Task Import(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string content = stream.ReadToEnd();
                
                    List<MovieDAO> movies = JsonConvert.DeserializeObject<List<MovieDAO>>(content) ?? new List<MovieDAO>();

                    await _movieRepository.DeleteAll();

                    await _movieRepository.Save(movies);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading file.");
                throw;
            }
        }
    }

}