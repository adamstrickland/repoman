using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;
using Repoman.Core;

namespace Repoman.Core.Testing
{
    public class TestHarnessRepositoryFactory<TContext> : IRepositoryFactory<TContext>
        where TContext : ObjectContext
    {
        private readonly TContext _context;
        private readonly IDictionary<string, ITestHarnessRepository> _repositoryByName = new Dictionary<string, ITestHarnessRepository>();

        public TestHarnessRepositoryFactory()
        {
            // Call the string constructor.
            ConstructorInfo constructor = typeof(TContext).GetConstructor(new[] { typeof(string) });
            const string connectionString = "metadata=res://*;provider=System.Data.SqlClient;provider connection string=\"Data Source=.;Initial Catalog=master;Integrated Security=True\"";
            _context = (TContext)constructor.Invoke(new object[] { connectionString });
        }

        public IRepository<TEntity> GetRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> query)
            where TEntity : EntityObject
        {
            ITestHarnessRepository testHarnessRepository;
            string repositoryName = GetRepositoryName(query);
            if (!_repositoryByName.TryGetValue(repositoryName, out testHarnessRepository))
            {
                throw new Exception("The repository " + repositoryName + " has not been initialized in the test harness.");
            }

            return (TestHarnessRepository<TEntity>)testHarnessRepository;
        }

        public TContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public void AddObjectToRepository<TEntity>(
            Func<TContext, ObjectQuery<TEntity>> query,
            TEntity testObject)
            where TEntity : EntityObject
        {
            TestHarnessRepository<TEntity> repository = GetTestHarnessRepository(query);
            repository.Register(testObject);
        }

        public TestHarnessRepository<TEntity> InitializeRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> query) where TEntity : EntityObject
        {
            return GetTestHarnessRepository<TEntity>(query);
        }

        private TestHarnessRepository<TEntity> GetTestHarnessRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> query)
            where TEntity : EntityObject
        {
            ITestHarnessRepository testHarnessRepository;
            string repositoryName = GetRepositoryName(query);

            if (!_repositoryByName.TryGetValue(repositoryName, out testHarnessRepository))
            {
                testHarnessRepository = new TestHarnessRepository<TEntity>();
                _repositoryByName.Add(repositoryName, testHarnessRepository);
            }

            return (TestHarnessRepository<TEntity>)testHarnessRepository;
        }

        private string GetRepositoryName<TEntity>(Func<TContext, ObjectQuery<TEntity>> query)
            where TEntity : EntityObject
        {
            string commandText = query(_context).CommandText;
            return commandText.Substring(1, commandText.Length - 2);
        }
    }
}
