using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using Newtonsoft.Json;
using Repository;
using Repository.Interface;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TopMovieService : ITopMovieService
    {
        private readonly ITopMovieRepository _topMovieRepository;
        public TopMovieService(ITopMovieRepository topMovieRepository)
        {
            _topMovieRepository = topMovieRepository;
        }
        public async Task Create(TopMovie movie)
        {
            await _topMovieRepository.Create(movie);
        }
        public async Task<List<TopMovie>> GetByYears(int yearDate)
        {
            return await _topMovieRepository.GetByDate(yearDate);
        }

        public async Task<TopMovie> Get(Guid id)
        {
            return await _topMovieRepository.Get(id);
        }

        public async Task<TopMovie> Update(TopMovie topMovie)
        {
            return await _topMovieRepository.Update(topMovie);
        }

        public async Task Delete(Guid id)
        {
            await _topMovieRepository.Delete(id);
        }

        public async Task Import(IFormFile file)
        {
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string content = stream.ReadToEnd();

                    List<TopMovie> topMovies = JsonConvert.DeserializeObject<List<TopMovie>>(content) ?? new List<TopMovie>();
                    // First, delete all existing directors.
                    await _topMovieRepository.DeleteAll();

                    await _topMovieRepository.Save(topMovies);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FileContentResult> Download()
        {
            var topMovies = await _topMovieRepository.GetAll();

            var jsonString = JsonConvert.SerializeObject(topMovies);

            var content = Encoding.UTF8.GetBytes(jsonString);
            var contentType = "application/json";
            var fileName = "directors.json";

            return new FileContentResult(content, contentType) { FileDownloadName = fileName };
        }

    
    }
}
