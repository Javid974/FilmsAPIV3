using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DAO;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> Get(Guid id);
        Task<List<Movie>> GetByViewingYearsDate(int yearDate);
        Task<MovieDAO> Update(Movie movie);
        Task Create(Movie movie);
        Task Delete(Guid id);
        Task<string> Upload(IFormFile file);
        Task Import(IFormFile file);
        Task<FileContentResult> Download();
    }

}
