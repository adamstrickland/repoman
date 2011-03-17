using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repoman.Core
{
    public class EntityFrameworkQueryProvider : EntityFrameworkQueryLogger, IQueryProvider
    {
        private readonly IQueryable _query;

        public EntityFrameworkQueryProvider(IQueryable query)
        {
            _query = query;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new EntityFrameworkQuery<TElement>(_query.Provider.CreateQuery<TElement>(expression));
        }

        public IQueryable CreateQuery(Expression expression)
        {
            IQueryable query = _query.Provider.CreateQuery(expression);

            // Get the specific type of the query.
            Type queryType = query.GetType();
            Type[] arguments = queryType.GetGenericArguments();
            if (arguments.Length == 1)
            {
                // Construct the correct type of wrapper.
                Type wrapperType = typeof(EntityFrameworkQuery<>).MakeGenericType(arguments);
                Type queryableType = typeof(IQueryable<>).MakeGenericType(arguments);
                ConstructorInfo constructor = wrapperType.GetConstructor(new[] { queryableType });
                if (constructor != null)
                    return (IQueryable)constructor.Invoke(new object[] { query });
            }

            return query;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return LogQuery(_query, () => _query.Provider.Execute<TResult>(expression));
        }

        public object Execute(Expression expression)
        {
            return LogQuery(_query, () => _query.Provider.Execute(expression));
        }
    }
}
