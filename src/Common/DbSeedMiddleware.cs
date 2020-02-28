using System;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public class DbSeedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _dbConnectionString;
        private readonly string _dbServerConnectionString;
        private readonly string _dbName;

        public DbSeedMiddleware(RequestDelegate next, 
            string connectionStringName,
            IConfiguration config)
        {
            _next = next;
            _dbConnectionString = config.GetConnectionString(connectionStringName);

            var builder = new SqlConnectionStringBuilder(_dbConnectionString);
            _dbName = builder.InitialCatalog;
            builder.InitialCatalog = string.Empty;
            _dbServerConnectionString = builder.ToString();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                using (var conn = new SqlConnection(_dbConnectionString))
                {
                    await conn.ExecuteAsync("select 1");
                }
            }
            catch (SqlException)
            {
                await CreateDatabase();
                await SeedDatabase();
            }

            await _next(context);
        }

        private async Task CreateDatabase()
        {
            using (var conn = new SqlConnection(_dbServerConnectionString))
            {
                await conn.ExecuteAsync($"create database {_dbName}");
            }

            var path = Path.Combine("Migrations", "Initial.sql");
            if (!File.Exists(path))
            {
                return;
            }

            using (var conn = new SqlConnection(_dbConnectionString))
            {
                foreach (var statement in File.ReadAllText(path).Split("GO"))
                {
                    if (!string.IsNullOrEmpty(statement))
                    {
                        await conn.ExecuteAsync(statement);
                    }
                }
            }
        }

        private async Task SeedDatabase()
        {
            var path = Path.Combine("DbSeed", "Initial.sql");
            if (!File.Exists(path))
            {
                return;
            }

            using (var conn = new SqlConnection(_dbConnectionString))
            {
                foreach (var statement in File.ReadAllText(path).Split("GO"))
                {
                    if (!string.IsNullOrEmpty(statement))
                    {
                        await conn.ExecuteAsync(statement);
                    }
                }
            }
        }
    }

    public static class UserDbSeedMiddlewareExtensions
    {
        public static IApplicationBuilder UseDbSeed(this IApplicationBuilder builder, string connectionStringName)
        {
            return builder.UseMiddleware<DbSeedMiddleware>(connectionStringName);
        }
    }
}
