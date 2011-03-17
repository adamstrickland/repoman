using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using Repoman.Core;

namespace Repoman.Core.Testing
{
    public class TestHarnessRepository<TEntity> : IRepository<TEntity>, ITestHarnessRepository
         where TEntity : EntityObject
    {
        private readonly Collection<TEntity> _entities = new Collection<TEntity>();
        private readonly Collection<Predicate<TEntity>> _expectedRegisteredEntities = new Collection<Predicate<TEntity>>();
        private readonly Collection<Predicate<TEntity>> _expectedRemovedEntities = new Collection<Predicate<TEntity>>();

        public TEntity First()
        {
            return _entities.First();
        }

        public TEntity Last()
        {
            return _entities.Last();
        }

        public IQuerySource<TEntity> GetSatisfying(Func<IQueryable<TEntity>, IQueryable<TEntity>> specification)
        {
            return new TestHarnessQuerySource<TEntity>(_entities.AsQueryable(), specification);
        }

        public IQuerySource<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> predicate)
        {
            return new TestHarnessQuerySource<TEntity>(_entities.AsQueryable(), r => r.Where(predicate));
        }

        public void Register(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void Remove(EntityObject entity)
        {
            _entities.Remove((TEntity)entity);
        }

        //public void AddObject(TEntity testObject)
        //{
        //    _entities.Add(testObject);
        //}
    }
}
