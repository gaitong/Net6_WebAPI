using Dapper;
using Microsoft.Extensions.Configuration;
using net6_template_devstandard_api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Repositories
{

    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<int> Add(Movie model);
        Task<int> Update(Movie model);
        Task<int> Delete(int id);
    }
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public override async Task<int> Add(Movie model)
        {
            var sqlCommand = string.Format(@"INSERT INTO [dbo].[Movie]
                                                   ([Title]
                                                   ,[Url])
                                             VALUES
                                                   (@Title
                                                   ,@Url)");
            using (var db = new SqlConnection(connectionStrings))
            {
                return await db.ExecuteAsync(sqlCommand, MappingParameter(model));
            }
        }
        public override async Task<int> Update(Movie model)
        {
            var sqlCommand = string.Format(@"UPDATE [Movie]
                                               SET [Title] = @Title
                                                  ,[Url] = @Url
                                             WHERE [Id] = @Id");
            using (var db = new SqlConnection(connectionStrings))
            {
                return await db.ExecuteAsync(sqlCommand, MappingParameter(model));
            }
        }
        public override async Task<int> Delete(int id)
        {
            var sqlCommand = string.Format(@"DELETE FROM [dbo].[Movie] WHERE [Id] = @Id");
            using (var db = new SqlConnection(connectionStrings))
            {
                return await db.ExecuteAsync(sqlCommand, new { Id = id });
            }
        }
        private object MappingParameter(Movie model)
        {
            return new
            {
                Id = model.Id,
                Title = model.Title,
                Url = model.Url
            };
        }
    }
}
