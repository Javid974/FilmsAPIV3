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
    public interface ITopMovieService
    {
        Task Create(TopMovie topMovie);
        Task<List<TopMovie>> GetByYears(int yearDate);
        Task<TopMovie> Get(Guid id);
        Task<TopMovie> Update(TopMovie topMovie);
         Task Delete(Guid id);
        Task Import(IFormFile file);
        Task<FileContentResult> Download();
    }
}
