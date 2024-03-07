using Models;
using Models.DAO;
using MongoDB.Driver;
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
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;

        public DirectorService(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }
        public async Task<Director> Create(Director director)
        {
            try
            {
                return await _directorRepository.Save(director);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<Director?> GetByName(string name)
        {
            return await _directorRepository.GetByName(name);
        }

        public async Task<Director?> GetById(int id)
        {
            return await _directorRepository.GetById(id);
        }
    }
}
