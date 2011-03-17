using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repoman.Core
{
    public class EntityFrameworkQuery<TEntity> : EntityFrameworkQueryLogger, IOrderedQueryable<TEntity>
    {
        private readonly IQueryable<TEntity> _query;

        public EntityFrameworkQuery(IQueryable<TEntity> query)
        {
            _query = query;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return LogQuery(_query, () => _query.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return LogQuery(_query, () => _query.GetEnumerator());
        }

        public Type ElementType
        {
            get { return _query.ElementType; }
        }

        public Expression Expression
        {
            get { return _query.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return new EntityFrameworkQueryProvider(_query); }
        }
    }
}
