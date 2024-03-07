using Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ITopMovieRepository
    {
        Task<TopMovie> Create(TopMovie movie);
        Task<TopMovie> Get(Guid id);
        Task<TopMovie> Update(TopMovie movie);
        Task Save(List<TopMovie> movies);
        Task<List<TopMovie>> GetByDate(int yearsDate);
        Task Delete(Guid id);
        Task DeleteAll();
        Task<List<TopMovie>> GetAll();
    }
}
