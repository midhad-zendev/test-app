using System;
using System.Data;

namespace Example.Db
{
    public class ConnectionContainer : IDisposable
    {
        private IDbTransaction _transaction;

        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                if (_transaction != null)
                    Connection = _transaction.Connection;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }

            _transaction = null;
            Connection = null;
        }
    }
}
