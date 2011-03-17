using System.Data.Objects;
using System;
using Repoman.Core;

namespace Repoman.Core.Testing
{
    public class TestHarness : IDisposable
    {
        private readonly TestHarnessTransaction _testHarnessTransaction = new TestHarnessTransaction();

        public TestHarnessContext<TContext> UsingContext<TContext>()
            where TContext : ObjectContext
        {
            return _testHarnessTransaction.InternalUsingContext<TContext>();
        }

        public ITransaction CreateMockTransaction()
        {
            return _testHarnessTransaction;
        }

        public void Dispose()
        {
            _testHarnessTransaction.Dispose();
        }
    }
}
