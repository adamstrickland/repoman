using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Reflection;


namespace Repoman.Core
{
    public class EntityFrameworkRepositoryFactory<TContext> : IRepositoryFactory<TContext>, IEntityFrameworkObjectContextOwner
        where TContext : ObjectContext
    {
        private readonly TContext _context;
        private readonly IDictionary<Type, object> _repositoryByEntityType = new Dictionary<Type, object>();

        public EntityFrameworkRepositoryFactory(TContext context)
        {
            _context = context;
            // _context.Connection.Open();
        }

        public IRepository<TEntity> GetRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> root)
            where TEntity : EntityObject
        {
            // See if we've already created this repository.
            object repository;
            if (_repositoryByEntityType.TryGetValue(typeof(TEntity), out repository))
            {
                return repository as IRepository<TEntity>;
            }

            // Add a new repository.
            var newRepository = new EntityFrameworkRepository<TEntity>(_context, root(_context));
            _repositoryByEntityType.Add(typeof(TEntity), newRepository);
            return newRepository;
        }

        public ObjectContext ObjectContext
        {
            get { return _context; }
        }

        public TContext Context
        {
            get { return _context; }
        }
    }
}
