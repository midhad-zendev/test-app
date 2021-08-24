using Example.Db.Interfaces;
using Dapper;
using System;
using Microsoft.Data.Sqlite;
using System.Data;
using Example.Db.Repositories;

namespace Example.Db
{
    public class DbContext : IDbContext, IDisposable
    {
        private ConnectionContainer _connectionContainer;
        private UsersRepository _usersService;


        static DbContext()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            //Declare type handlers ex. List<string> store as json in table column and deserialize when using
           // SqlMapper.AddTypeHandler(typeof(List<string>), new ObjListType());
     
        }

        public DbContext(string connectionString, bool leaveConnectionOpen = false)
        {
            
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            // SqlMapper.AddTypeHandler(typeof(MsisdnList), new MsisdnListType());
            _connectionContainer = new ConnectionContainer
            {
                Connection = new SqliteConnection(connectionString)
            };
            LeaveConnectionOpen = leaveConnectionOpen;

        }

        public ConnectionContainer DbConnectionContainer => _connectionContainer;

        public bool LeaveConnectionOpen { get; }
        
        //Introduce new repositories
        public UsersRepository UsersService => _usersService ?? (_usersService = new UsersRepository(_connectionContainer, LeaveConnectionOpen));

        public IDbTransaction BeginTransaction()
        {
            DbConnectionContainer.Transaction = DbConnectionContainer.Connection.BeginTransaction();
            return DbConnectionContainer.Transaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            DbConnectionContainer.Transaction = DbConnectionContainer.Connection.BeginTransaction(isolationLevel);
            return DbConnectionContainer.Transaction;
        }

        public void CommitTransaction()
        {
            DbConnectionContainer.Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            DbConnectionContainer.Transaction.Rollback();
        }

        public void Dispose()
        {
            _usersService?.Dispose();
            _usersService = null;
   
            _connectionContainer?.Dispose();
            _connectionContainer = null;
        }
    }
}
