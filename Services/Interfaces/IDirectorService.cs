using Models;
using Models.DAO;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDirectorService
    {
        Task<Director> Create(Director movie);

        Task<Director?> GetByName(string name);
    }
}
