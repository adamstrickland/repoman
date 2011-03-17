using System;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;

namespace Repoman.Core
{
    public interface IRepository<TEntity>
        where TEntity : EntityObject
    {
        IQuerySource<TEntity> GetSatisfying(Func<IQueryable<TEntity>, IQueryable<TEntity>> specification);
        IQuerySource<TEntity> GetSatisfying(Expression<Func<TEntity, Boolean>> predicate);
        TEntity First();
        TEntity Last();
        void Register(TEntity entity);
        void Remove(EntityObject entity);
    }
}
