using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace Repoman.Core.Testing
{
    public class TestHarnessContext<TContext> : ITestHarnessContext
        where TContext : ObjectContext
    {
        readonly TestHarnessRepositoryFactory<TContext> _repositoryFactory = new TestHarnessRepositoryFactory<TContext>();

        public TestHarnessContext<TContext> AddObjectTo<TEntity>(
            Func<TContext, ObjectQuery<TEntity>> repository,
            TEntity testObject)
            where TEntity : EntityObject
        {
            _repositoryFactory.AddObjectToRepository(repository, testObject);
            return this;
        }

        public TestHarnessContext<TContext> InitializeRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> repository)
            where TEntity : EntityObject
        {
            _repositoryFactory.InitializeRepository<TEntity>(repository);
            return this;
        }

        //public TestHarnessContext<TContext> ExpectObjectRegisteredIn<TEntity>(
        //    Func<TContext, ObjectQuery<TEntity>> repository,
        //    TEntity testObject)
        //    where TEntity : EntityObject
        //{
        //    _repositoryFactory.ExpectObjectRegisteredIn(repository, a => a.Equals(testObject));
        //    return this;
        //}

        //public TestHarnessContext<TContext> ExpectObjectRegisteredIn<TEntity>(
        //    Func<TContext, ObjectQuery<TEntity>> repository,
        //    Predicate<TEntity> predicate)
        //    where TEntity : EntityObject
        //{
        //    _repositoryFactory.ExpectObjectRegisteredIn(repository, predicate);
        //    return this;
        //}

        //public TestHarnessContext<TContext> ExpectObjectRemovedFrom<TEntity>(
        //    Func<TContext, ObjectQuery<TEntity>> repository,
        //    TEntity testObject)
        //    where TEntity : EntityObject
        //{
        //    _repositoryFactory.ExpectObjectRemovedFrom(repository, a => a.Equals(testObject));
        //    return this;
        //}

        //public TestHarnessContext<TContext> ExpectObjectRemovedFrom<TEntity>(
        //    Func<TContext, ObjectQuery<TEntity>> repository,
        //    Predicate<TEntity> predicate)
        //    where TEntity : EntityObject
        //{
        //    _repositoryFactory.ExpectObjectRemovedFrom(repository, predicate);
        //    return this;
        //}

        //public void ValidateAll()
        //{
        //    _repositoryFactory.ValidateAll();
        //}

        internal TestHarnessRepositoryFactory<TContext> GetMockRepositoryFactory()
        {
            return _repositoryFactory;
        }
    }
}
