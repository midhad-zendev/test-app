using Example.Db.Repositories;
using System.Data;

namespace Example.Db.Interfaces
{
    public interface IDbContext
    {
        ConnectionContainer DbConnectionContainer { get; }

        UsersRepository UsersService { get; }
       
        bool LeaveConnectionOpen { get; }

        IDbTransaction BeginTransaction();

        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void RollbackTransaction();
        void Dispose();
    }
}
