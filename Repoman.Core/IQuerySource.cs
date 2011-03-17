using System;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;

namespace Repoman.Core
{
    /// <summary>
    /// A builder for queries.
    /// </summary>
    /// <typeparam name="TEntity">The root of repository that we are querying.</typeparam>
    public interface IQuerySource<TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the root and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the root and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, EntityCollection<TRelated>>> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the root and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the root and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, TRelated>> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the root and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the root and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IQuerySource<TEntity> Include<TRelated>(
            Expression<Func<TEntity, IIncludeSpec<TRelated>>> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Generate a query.
        /// </summary>
        /// <returns>Converts the query source into an IQueryable.</returns>
        IQueryable<TEntity> Query();
    }
}
