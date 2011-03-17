using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;

namespace Repoman.Core
{
    /// <summary>
    /// A nested specification of includes.
    /// </summary>
    /// <typeparam name="TEntity">The parent type of this include tree.</typeparam>
    public interface IIncludeSpec<out TEntity>
        where TEntity : EntityObject
    {
        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the parent and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the parent and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IIncludeSpec<TEntity> Include<TRelated>(
            Func<TEntity, EntityCollection<TRelated>> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the parent and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the parent and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IIncludeSpec<TEntity> Include<TRelated>(
            Func<TEntity, TRelated> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Include related entities from a navigation property. Pass in a lambda expression
        /// that takes the parent and returns the navigation property. For example, n =&gt; n.Topic
        /// </summary>
        /// <typeparam name="TRelated">!!!DON'T SPECIFY!!! This type is inferred.</typeparam>
        /// <param name="navigationProperty">A lambda expression that takes the parent and returns the navigation property.
        /// For example, n =&gt; n.Topic</param>
        /// <returns>Returns this to support chaining.</returns>
        IIncludeSpec<TEntity> Include<TRelated>(
            Func<TEntity, IIncludeSpec<TRelated>> navigationProperty)
            where TRelated : EntityObject;

        /// <summary>
        /// Get all of the paths specified by the nested includes.
        /// </summary>
        IEnumerable<string> Paths { get; }
    }
}
