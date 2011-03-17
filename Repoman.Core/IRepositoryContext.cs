using System.Collections.ObjectModel;
using System.Data.Objects;
namespace Repoman.Core
{
    /// <summary>
    /// Starting point for data access.
    /// </summary>
    public interface IRepositoryContext
    {
        /// <summary>
        /// <para>Wrap a set of atomic changes, or consistent queries. Call this method inside of a using statement. Before the end of
        /// the block, call commit to ensure that all changes are written to the database. If an exception is thrown or you do
        /// not call commit, the transaction is rolled back.</para>
        /// <para>Begin a transaction even if no changes are required. After a set of queries, do not call commit, as there are no changes
        /// to write.</para>
        /// </summary>
        /// <returns></returns>
        ITransaction BeginTransaction();
    }
}
