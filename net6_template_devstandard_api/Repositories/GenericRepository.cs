using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Repositories
{
    public abstract class GenericRepository<T>
    {
        private readonly IConfiguration configuration;
        protected readonly string connectionStrings = "";
        public GenericRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionStrings = configuration.GetSection("ConnectionStrings:AppDB").Value;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var className = typeof(T).Name;
            var sqlCommand = $"SELECT * FROM {className}";
            using (var db = new SqlConnection(connectionStrings))
            {
                var result = await db.QueryAsync<T>(sqlCommand);
                return result.ToList();
            }
        }

        public async Task<T> GetById(int id)
        {
            var className = typeof(T).Name;
            var sqlCommand = $"SELECT * FROM {className} WHERE [Id] = @id";
            using (var db = new SqlConnection(connectionStrings))
            {
                var result = await db.QueryAsync<T>(sqlCommand, new { id = id });
                return result.FirstOrDefault();
            }
        }
        public abstract Task<int> Add(T model);
        public abstract Task<int> Update(T model);
        public abstract Task<int> Delete(int id);
    }
}
