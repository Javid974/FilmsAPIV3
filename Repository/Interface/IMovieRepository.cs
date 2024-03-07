using Models;
using Models.DAO;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IMovieRepository
    {
        Task<MovieDAO?> GetById(int id);

        Task<Movie> GetById(Guid id);

        Task<List<Movie>> GetByViewingYearsDate(int yearsDate);
        Task<MovieDAO> Create(MovieDAO movie);
        Task<MovieDAO> Update(MovieDAO movie);
        Task Save(List<MovieDAO> movies);
        Task<List<MovieDAO>> GetAll();
        Task<MightWatchMovieDAO> Create(MightWatchMovieDAO movie);
        Task Delete(Guid id);
        Task DeleteAll();
        Task<List<DirectorMoviesCount>> GetDirectorMoviesCount();
        Task<List<Movie>> GetByDirectorId(int directorId);

    }
}
