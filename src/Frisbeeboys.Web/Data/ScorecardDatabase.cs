using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Frisbeeboys.Web.Data
{
    public class ScorecardDatabase
    {
        private readonly string _connectionString;

        public ScorecardDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<NpgsqlConnection> GetOpenConnectionAsync()
        {
            var cnn = new NpgsqlConnection(_connectionString);
            await cnn.OpenAsync();
            return cnn;
        }

        public async Task<IList<T>> QueryAsync<T>(string query, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return (await cnn.QueryAsync<T>(query, param)).ToList();
        }

        public async Task<T> QuerySingleAsync<T>(string query, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return await cnn.QuerySingleAsync<T>(query, param);
        }
        
        public async Task<T> QuerySingleOrDefaultAsync<T>(string query, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return await cnn.QuerySingleOrDefaultAsync<T>(query, param);
        }
        
        public async Task<T> QueryFirstAsync<T>(string query, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return await cnn.QueryFirstAsync<T>(query, param);
        }
        
        public async Task<T> QueryFirstOrDefaultAsync<T>(string query, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return await cnn.QueryFirstOrDefaultAsync<T>(query, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            await using var cnn = await GetOpenConnectionAsync();
            return await cnn.ExecuteAsync(sql, param);
        }

        public QueryBuilder<T> Query<T>(string? alias = null)
        {
            return new(GetOpenConnectionAsync, alias);
        }
    }
}