using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DAO;
using Models.ViewModel;
using Newtonsoft.Json;
using Repository;
using Repository.Interface;
using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        public StatisticsService(IMovieRepository movieRepository, IDirectorRepository directorRepository)
        {
            _movieRepository = movieRepository;
            _directorRepository = directorRepository;
        }
        public async Task<Statistics> Get(int yearDate)
        {
            var moviesCountPerCountry = new Dictionary<string, Tuple<Country, int>>();
            var movies = await _movieRepository.GetByViewingYearsDate(yearDate);
            var watchedMovies = movies.Where(movie => movie.MightWatch.HasValue && !movie.MightWatch.Value).ToList();
            var countries = watchedMovies
                .SelectMany(m => m.Countries)
                .GroupBy(c => c.Iso_3166_1)  // Assume that Id is unique for each Country
                .Select(g => g.First())
                .ToList();
            var statistics = new Statistics();
            statistics.Year = yearDate;
            statistics.CalculateStatistics(watchedMovies, countries);

            return statistics;
        }

        public async Task<List<DirectorMoviesCount>> GetDirectorMoviesCount()
        {
            return await _movieRepository.GetDirectorMoviesCount();
        }

        public async Task<Director?> GetDirectorById(int directorId)
        {
            return await _directorRepository.GetById(directorId);
        }

        public async Task<List<Movie>> GetMoviesByDirectorId(int directorId)
        {
            return await _movieRepository.GetByDirectorId(directorId);
        }

        public async Task<FileContentResult> Download()
        {
            var directors =  await _directorRepository.GetAll();

            var jsonString = JsonConvert.SerializeObject(directors);

            var content = Encoding.UTF8.GetBytes(jsonString);
            var contentType = "application/json";
            var fileName = "directors.json";

            return new FileContentResult(content, contentType) { FileDownloadName = fileName };
        }

        public async Task Import(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string content = stream.ReadToEnd();

                    List<Director> directors = JsonConvert.DeserializeObject<List<Director>>(content) ?? new List<Director>();
                    // First, delete all existing directors.
                    await _directorRepository.DeleteAll();

                    await _directorRepository.Save(directors);

                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
