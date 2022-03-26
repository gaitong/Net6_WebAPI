using net6_template_devstandard_api.Models;
using net6_template_devstandard_api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<bool> Add(Movie model);
        Task<bool> Update(Movie model);
        Task<bool> Delete(int id);
    }
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await movieRepository.GetAll();
        }

        public async Task<Movie> GetById(int id)
        {
            return await movieRepository.GetById(id);
        }

        public async Task<bool> Add(Movie model)
        {
            var movies = movieRepository.GetAll().Result.ToList();
            var isDuplicate = movies.Where(m => string.Equals(m.Title, model.Title, StringComparison.OrdinalIgnoreCase));
            if (isDuplicate.Count() > 0)
            {
                throw new Exception("Data is duplicate!");
            }
            var res = await movieRepository.Add(model);
            return res > 0;
        }

        public async Task<bool> Update(Movie model)
        {
            var movies = movieRepository.GetAll().Result.ToList();
            var isDuplicate = movies.Where(m => string.Equals(m.Title, model.Title, StringComparison.OrdinalIgnoreCase) && m.Id != model.Id);
            if (isDuplicate.Count() > 0)
            {
                throw new Exception("Data is duplicate!");
            }
            var res = await movieRepository.Update(model);
            return res > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var movie = movieRepository.GetById(id);
            if (movie == null)
            {
                throw new Exception("Data is not exist!");
            }
            var res = await movieRepository.Delete(id);
            return res > 0;
        }
    }
}
