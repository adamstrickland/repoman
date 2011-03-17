using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using Repoman.Core;

namespace Repoman.Core.Testing
{
    public class TestHarnessTransaction : ITransaction, IDisposable
    {
        private readonly IDictionary<Type, ITestHarnessContext> _contextByType = new Dictionary<Type, ITestHarnessContext>();
        private bool disposed = false;

        ~TestHarnessTransaction()
        {
            Dispose(false);
        }
        
        public IRepositoryFactory<TContext> UsingContext<TContext>()
            where TContext : ObjectContext
        {
            if (disposed)
                throw new ObjectDisposedException("TestHarnessTransaction");

            //Find the object context
            ITestHarnessContext context;
            if (!_contextByType.TryGetValue(typeof(TContext), out context))
            {
                throw new InvalidOperationException("The context type " + typeof(TContext).Name + " has not been initialized in the test harness");
            }

            return ((TestHarnessContext<TContext>)context).GetMockRepositoryFactory();
        }

        public void ExecuteNonQuery<TContext>(string commandText, CommandType commandType, params object[] parameters) where TContext : ObjectContext
        {
            if (disposed)
                throw new ObjectDisposedException("TestHarnessTransaction");

            throw new NotImplementedException();
        }

        public void Commit()
        {
            if (disposed)
                throw new ObjectDisposedException("TestHarnessTransaction");            
        }

        public void SaveChanges()
        {
            if (disposed)
                throw new ObjectDisposedException("TestHarnessTransaction");

            throw new NotImplementedException();
        }

        public TestHarnessContext<TContext> InternalUsingContext<TContext>()
            where TContext : ObjectContext
        {
            if (disposed)
                throw new ObjectDisposedException("TestHarnessTransaction");

            // Get the context, or create it if it isn't there yet.
            ITestHarnessContext context;
            if (!_contextByType.TryGetValue(typeof(TContext), out context))
            {
                context = new TestHarnessContext<TContext>();
                _contextByType.Add(typeof(TContext), context);
            }

            return (TestHarnessContext<TContext>)context;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            try
            {
                if (disposing)
                {
                    // Dispose contained managed objects here
                }

                if (disposed)
                    return;

                // Release native (non-managed) objects here.

                // Call GC.SuppressFinalize(this) to prevent Finalize from being called
                GC.SuppressFinalize(this);
            }
            finally
            {
                disposed = true;
            }
        }
    }
}
