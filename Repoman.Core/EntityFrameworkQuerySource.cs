using System;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;


namespace Repoman.Core
{
    public class EntityFrameworkQuerySource<TEntity> : IQuerySource<TEntity>
        where TEntity : EntityObject
    {
        private ObjectQuery<TEntity> _objectQuery;
        private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>> _specification;

        public EntityFrameworkQuerySource(ObjectQuery<TEntity> objectQuery, Func<IQueryable<TEntity>, IQueryable<TEntity>> specification)
        {
            _objectQuery = objectQuery;
            _specification = specification;
        }

        public IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, EntityCollection<TRelated>>> navigationProperty)
            where TRelated : EntityObject
        {
            tree(null, navigationProperty.Body);
            return this;
        }

        public IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, IIncludeSpec<TRelated>>> navigationProperty)
            where TRelated : EntityObject
        {
            tree(null, navigationProperty.Body);
            return this;
        }

        public IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, TRelated>> navigationProperty)
            where TRelated : EntityObject
        {
            tree(null, navigationProperty.Body);
            return this;
        }

        private void tree(string scope, Expression body)
        {
            // If the body of the lambda is a member expression, terminate.
            var memberAccess = body as MemberExpression;
            if (memberAccess != null)
            {
                IncludeNavigationProperty(scope, memberAccess.Member.Name);
            }
            else
            {
                // If the body of the lambda is a method call, branch.
                MethodCallExpression methodCall = body as MethodCallExpression;
                if (methodCall == null)
                    throw new ArgumentException();

                branch(scope, methodCall);
            }
        }

        private string branch(string scope, MethodCallExpression methodCall)
        {
            ReadOnlyCollection<Expression> arguments = methodCall.Arguments;
            Expression right;
            if (arguments.Count == 1)
            {
                // Drill down to the member access on the left.
                MethodCallExpression leftMethodCall = methodCall.Object as MethodCallExpression;
                if (leftMethodCall == null)
                    throw new ArgumentException();
                scope = branch(scope, leftMethodCall);

                // Include all children of the parent.
                right = arguments[0];
            }
            else if (arguments.Count == 2)
            {
                // The left-hand side must be a member access.
                MemberExpression body = arguments[0] as MemberExpression;
                if (body == null)
                    throw new ArgumentException();
                scope = IncludeNavigationProperty(scope, body.Member.Name);

                // Include all children of the parent.
                right = arguments[1];
            }
            else
                throw new ArgumentException();

            // The right-hand side is a lambda, and the root of a new tree.
            LambdaExpression root = right as LambdaExpression;
            if (root == null)
                throw new ArgumentException();
            tree(scope, root.Body);

            return scope;
        }

        private string IncludeNavigationProperty(string scope, string name)
        {
            scope = (scope == null) ? name : scope + "." + name;
            _objectQuery = _objectQuery.Include(scope);
            return scope;
        }

        public IQueryable<TEntity> Query()
        {
            return new EntityFrameworkQuery<TEntity>(_specification(_objectQuery));
        }
    }
}
