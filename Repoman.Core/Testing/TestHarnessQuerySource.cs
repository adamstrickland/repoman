using System;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using Repoman.Core;

namespace Repoman.Core.Testing
{
    public class TestHarnessQuerySource<TEntity> : IQuerySource<TEntity>
        where TEntity : EntityObject
    {
        private readonly IQueryable<TEntity> _entities;
        private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>> _specification;

        public TestHarnessQuerySource(IQueryable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable<TEntity>> specification)
        {
            _entities = entities;
            _specification = specification;
        }

        public IQuerySource<TEntity> Include<TRelated>(Expression<Func<TEntity, EntityCollection<TRelated>>> navigationProperty) where TRelated : EntityObject
        {
            return this;
        }

        public IQuerySource<TEntity> Include<TRelated>(Expression<Func<TEntity, IIncludeSpec<TRelated>>> navigationProperty) where TRelated : EntityObject
        {
            return this;
        }

        public IQuerySource<TEntity> Include<TRelated>(Expression<Func<TEntity, TRelated>> navigationProperty) where TRelated : EntityObject
        {
            return this;
        }

        public IQueryable<TEntity> Query()
        {
            return _specification(_entities);
        }
    }
}
