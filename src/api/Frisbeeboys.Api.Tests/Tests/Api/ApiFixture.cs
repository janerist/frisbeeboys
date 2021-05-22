using System.IO;
using System.Net.Http;
using Dapper;
using Frisbeeboys.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

namespace Frisbeeboys.Api.Tests.Tests.Api
{
    public class ApiFixture : WebApplicationFactory<Startup>
    {
        public HttpClient Client { get; }
        public ScorecardDatabase Database { get; }

        private const string ConnectionString =
            "Server=localhost;Port=5432;Database=frisbeeboys_test;Username=postgres";
        
        public ApiFixture()
        {
            CreateTestDatabase();
         
            Server.PreserveExecutionContext = true;
            
            Client = CreateClient();
            Database = Services.GetService<ScorecardDatabase>();
        }

        private void CreateTestDatabase()
        {
            var cnnStringBuilder = new NpgsqlConnectionStringBuilder(ConnectionString);
            
            // Create test database
            using (var cnn = new NpgsqlConnection("Server=localhost;Port=5432;Database=postgres;Username=postgres"))
            {
                cnn.Open();
                cnn.Execute($"DROP DATABASE IF EXISTS {cnnStringBuilder.Database}");
                cnn.Execute($"CREATE DATABASE {cnnStringBuilder.Database}");
            }

            using (var cnn = new NpgsqlConnection(ConnectionString))
            {
                cnn.Open();
                cnn.Execute(File.ReadAllText("../../../../Frisbeeboys.Api/Data/Scripts/schema.sql"));
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Override database service with test database settings
                services.AddSingleton(new ScorecardDatabase(ConnectionString));
            });
        }
    }

    [CollectionDefinition("Api Tests")]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {
    }
}