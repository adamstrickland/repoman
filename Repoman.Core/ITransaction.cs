using System;
using System.Data;
using System.Data.Objects;

namespace Repoman.Core
{
    /// <summary>
    /// A container for database queries or changes. Pass the transaction into the application layer.
    /// Call UsingContext&lt;EDMX Class&gt;() to get repositories from an EDMX file. Before leaving
    /// the using block, call Commit() to save changes. If you only performed queries, do not
    /// call Commit(). If an exception is thrown before you call Commit(), all changes will be
    /// rolled back.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Use an Entity Framework context (EDMX), get a repository. Specify the context class fo the EDMX file. 
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        IRepositoryFactory<TContext> UsingContext<TContext>()
            where TContext : ObjectContext;

        /// <summary>
        /// Executes database stored procedure or command that returns no entity on specified repository context.
        /// </summary>
        /// <typeparam name="TContext">The context type of the EDMX file. Specified as "Entity Container Name". </typeparam>
        /// <param name="commandText">Text of the command to execute or stored procedure name.</param>
        /// <param name="commandType">Command type to execute.</param>
        /// <param name="parameters">Parameters for the command.</param>
        void ExecuteNonQuery<TContext>(string commandText, CommandType commandType, params object[] parameters)
            where TContext : ObjectContext;

        /// <summary>
        /// Commit all changes in the transaction. Do this at the end of the using block. If you are doing deletes
        /// and inserts in the same transaction, you may need to commit between the deletes and the inserts.
        /// </summary>
        void Commit();

        /// <summary>
        /// Persists all updates to the data source and resets change tracking in the object context, but does not
        /// commit the transaction. Could be used if you need to retrieve the ID of a new item, but you
        /// are not ready to commit the entire transaction. 
        /// </summary>
        void SaveChanges();
    }
}
