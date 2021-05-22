using System;
using System.Net.Http;
using System.Transactions;
using Frisbeeboys.Api.Data;

namespace Frisbeeboys.Api.Tests.Tests.Api
{
    public abstract class ApiTestBase : IDisposable
    {
        protected HttpClient Client { get; }
        protected ScorecardDatabase Database { get; }

        private readonly TransactionScope _transactionScope;
        
        protected ApiTestBase(ApiFixture fixture)
        {
            Client = fixture.Client;
            Database = fixture.Database;

            _transactionScope = TransactionFactory.CreateTransactionScope();
        }
        
        public void Dispose()
        {
            _transactionScope.Dispose();
        }
    }
}