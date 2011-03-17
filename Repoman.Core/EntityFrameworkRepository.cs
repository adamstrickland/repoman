using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;


namespace Repoman.Core
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
            where TEntity : EntityObject
    {
        private readonly ObjectContext _context;
        private readonly ObjectQuery<TEntity> _objectQuery;

        public EntityFrameworkRepository(ObjectContext context, ObjectQuery<TEntity> objectQuery)
        {
            _context = context;
            _objectQuery = objectQuery;
        }

        public TEntity First()
        {
            return _objectQuery.First();
        }

        public TEntity Last()
        {
            return _objectQuery.Last();
        }

        public IQuerySource<TEntity> GetSatisfying(Func<IQueryable<TEntity>, IQueryable<TEntity>> specification)
        {
            IQueryable<TEntity> q = _objectQuery;
            q.Where(e => true);
            return new EntityFrameworkQuerySource<TEntity>(_objectQuery, specification);
        }

        public IQuerySource<TEntity> GetSatisfying(Expression<Func<TEntity, bool>> predicate)
        {
            return new EntityFrameworkQuerySource<TEntity>(_objectQuery, r => r.Where(predicate));
        }

        public void Register(TEntity entity)
        {
            var entitySetName = _objectQuery.CommandText.Replace("[", "").Replace("]", "");
            _context.AddObject(entitySetName, entity);
        }

        public void Remove(EntityObject entity)
        {
            _context.DeleteObject(entity);
        }
    }
}
