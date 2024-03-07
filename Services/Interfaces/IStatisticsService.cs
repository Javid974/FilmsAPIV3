using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<Statistics> Get(int yearDate);
        Task<List<DirectorMoviesCount>> GetDirectorMoviesCount();
        Task<List<Movie>> GetMoviesByDirectorId(int directorId);
        Task<Director?> GetDirectorById(int directorId);
        Task<FileContentResult> Download();
        Task Import(IFormFile file);
    }
}
