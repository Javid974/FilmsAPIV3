using Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IDirectorRepository
    {
        Task<Director> Save(Director director);
        Task Save(List<Director> directors);
        Task<Director> Update(Director director);
        Task<Director> Delete(int id);
        Task DeleteAll();
        Task<Director?> GetByName(string name);
        Task<Director?> GetById(int id);
        Task<List<Director>> GetAll();

    }
}
