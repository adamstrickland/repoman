using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace Repoman.Core
{
    public interface IRepositoryFactory<TContext>
    {
        /// <summary>
        /// Get a repository from an EDMX file. Dont specifiy the TEntity parameter. Instead,
        /// use a lambda expression that, given a context, returns an entity collection. For example,
        /// c=&gt; c.Employee will get the collection of Employee entities from the collection
        /// </summary>
        /// <typeparam name="TEntity">!!!DON'T SPECIFY!!! This is inferred</typeparam>
        /// <param name="query">A lambda expression that gets an entity collection from and EDMX context.</param>
        /// <returns>A repository for the requested root.</returns>
        IRepository<TEntity> GetRepository<TEntity>(Func<TContext, ObjectQuery<TEntity>> query)
            where TEntity : EntityObject;

        /// <summary>
        /// Exposes underlying repository context.
        /// </summary>
        TContext Context { get; }
    }
}
