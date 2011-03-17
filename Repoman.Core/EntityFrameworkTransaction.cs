using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Transactions;
using Autofac;



namespace Repoman.Core
{
    public class EntityFrameworkTransaction : ITransaction, IDisposable
    {
        private readonly IDictionary<Type, object> _repositoryFactoryByContextType = new Dictionary<Type, object>();
        private readonly TransactionScope _scope;
        private bool disposed = false;

        public EntityFrameworkTransaction()
        {
            _scope = new TransactionScope();
        }

        ~EntityFrameworkTransaction()
        {
            Dispose(false);
        }

        public IRepositoryFactory<TContext> UsingContext<TContext>()
            where TContext : ObjectContext
        {
            if (disposed)
                throw new ObjectDisposedException("EntityFrameworkTransaction");

            // See if we've already created a repository factory in this transaction.
            object repositoryFactory;
            if (!_repositoryFactoryByContextType.TryGetValue(typeof(TContext), out repositoryFactory))
            {
                //repositoryFactory = new EntityFrameworkRepositoryFactory<TContext>(containerName, _connectionString);
                using (var container = new ContainerFactory<TContext>().Build())
                {
                    repositoryFactory = container.Resolve<EntityFrameworkRepositoryFactory<TContext>>();
                    _repositoryFactoryByContextType.Add(typeof(TContext), repositoryFactory);
                }
            }
            return repositoryFactory as IRepositoryFactory<TContext>;
        }

        //TODO: implement procs here
        public void ExecuteNonQuery<TContext>(string commandText, CommandType commandType, params object[] parameters)
            where TContext : ObjectContext
        {
            if (disposed)
                throw new ObjectDisposedException("EntityFrameworkTransaction");

            throw new NotSupportedException();
        }

        public void SaveChanges()
        {
            if (disposed)
                throw new ObjectDisposedException("EntityFrameworkTransaction");

            // Save changes in all contexts.
            foreach (object repository in _repositoryFactoryByContextType.Values)
            {
                var objectContextOwner = repository as IEntityFrameworkObjectContextOwner;
                // ReSharper disable PossibleNullReferenceException
                objectContextOwner.ObjectContext.SaveChanges();
                // ReSharper restore PossibleNullReferenceException
            }
        }

        public void Commit()
        {
            if (disposed)
                throw new ObjectDisposedException("EntityFrameworkTransaction");

            SaveChanges();
            _scope.Complete();
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
                    _scope.Dispose();
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
