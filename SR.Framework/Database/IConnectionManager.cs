using System.Data;

namespace SR.Framework.Database
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection();
    }
}
