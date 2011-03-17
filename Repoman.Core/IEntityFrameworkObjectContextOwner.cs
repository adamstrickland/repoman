using System.Data.Objects;

namespace Repoman.Core
{
    public interface IEntityFrameworkObjectContextOwner
    {
        ObjectContext ObjectContext { get; }
    }
}
