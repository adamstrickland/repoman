using System.Collections.Generic;


namespace Repoman.Core
{
    public class RepositoryContext : IRepositoryContext
    {
        public ITransaction BeginTransaction()
        {
            return new EntityFrameworkTransaction();
        }
    }
}
