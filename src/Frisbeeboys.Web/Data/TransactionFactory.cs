using System.Transactions;

namespace Frisbeeboys.Web.Data
{
    public static class TransactionFactory
    {
        public static TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = isolationLevel
            }, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}