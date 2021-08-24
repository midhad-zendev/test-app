using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Example.Db.Base
{
    public abstract class BaseRepository : IDisposable
    {
        public const int CommandTimeout = 1000 * 60 * 10; // 10 minutes
        public const int PageSize = 25;

        private ConnectionContainer _connectionContainer;
        private bool _leaveOpen;

        protected internal BaseRepository(ConnectionContainer container, bool leaveOpen)
        {
            _leaveOpen = leaveOpen;
            _connectionContainer = container;
        }

        public ConnectionContainer ConnectionContainer
        {
            get => _connectionContainer;
            protected set => _connectionContainer = value;
        }

        protected internal bool LeaveOpen
        {
            get => _leaveOpen;
            set => _leaveOpen = value;
        }

        public void Dispose()
        {
            if (!_leaveOpen)
            {
                _connectionContainer.Dispose();
            }
            _connectionContainer = null;
        }

        public IDbTransaction BeginTransaction()
        {
            _connectionContainer.Transaction = _connectionContainer.Connection.BeginTransaction();
            return _connectionContainer.Transaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _connectionContainer.Transaction = _connectionContainer.Connection.BeginTransaction(isolationLevel);
            return _connectionContainer.Transaction;
        }

        /// <summary>
        ///     Execute parameterized SQL
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public int Execute(string sql, dynamic param = null, IDbTransaction transaction = null,
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Execute(_connectionContainer.Connection, sql, param, transaction, commandTimeout,
                commandType);
            return query;
        }
        /// <summary>
        ///     ExecuteAsync parameterized SQL
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public async Task<int> ExecuteAsync(string sql, dynamic param = null, IDbTransaction transaction = null,
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.ExecuteAsync(_connectionContainer.Connection, sql, param, transaction, commandTimeout,
                commandType);
            return query;
        }

        /// <summary>
        ///     Return a list of dynamic objects, reader is closed after the call
        /// </summary>
        public IEnumerable<dynamic> Query(string sql, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Query(_connectionContainer.Connection, sql, param, transaction, buffered,
                commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///  Async  Return a list of dynamic objects, reader is closed after the call
        /// </summary>
        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync(_connectionContainer.Connection, sql, param, transaction,
                commandTimeout, commandType);
            return query;
        }


        /// <summary>
        ///     Executes a query, returning the data typed as per T
        /// </summary>
        /// <remarks>
        ///     the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs
        ///     completion gets annoying. Eg type new [space] get new object
        /// </remarks>
        /// <returns>
        ///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
        ///     column in assumed, otherwise an instance is
        ///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// </returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Query<T>(_connectionContainer.Connection, sql, param, transaction, buffered,
                commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///     Executes a query async, returning the data typed as per T
        /// </summary>
        /// <remarks>
        ///     the dynamic param may seem a bit odd, but this works around a major usability issue in vs, if it is Object vs
        ///     completion gets annoying. Eg type new [space] get new object
        /// </remarks>
        /// <returns>
        ///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
        ///     column in assumed, otherwise an instance is
        ///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
        /// </returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync<T>(_connectionContainer.Connection, sql, param, transaction,
                commandTimeout, commandType);
            return query;
        }

        /// <summary>
        ///     Maps a query to objects
        /// </summary>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Query<TFirst, TSecond, TReturn>(_connectionContainer.Connection, sql, map, param,
                transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///  Async  Maps a query to objects
        /// </summary>
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync<TFirst, TSecond, TReturn>(_connectionContainer.Connection, sql, map, param,
                transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }

        /// <summary>
        ///     Perform a multi mapping query with 5 input parameters
        /// </summary>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query =
                SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(_connectionContainer.Connection, sql,
                    map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///   Async  Perform a multi mapping query with 5 input parameters
        /// </summary>
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null,
            IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id",
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(_connectionContainer.Connection, sql,
                    map, param, transaction, buffered, splitOn,
                    commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///     Perform a multi mapping query with 4 input parameters
        /// </summary>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int commandTimeout = CommandTimeout,
            CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TReturn>(_connectionContainer.Connection,
                sql, map, param, transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///  Async   Perform a multi mapping query with 4 input parameters
        /// </summary>
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int commandTimeout = CommandTimeout,
            CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(_connectionContainer.Connection,
                sql, map, param, transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }

        /// <summary>
        ///     Maps a query to objects
        /// </summary>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int commandTimeout = CommandTimeout,
            CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(_connectionContainer.Connection, sql, map,
                param, transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }
        /// <summary>
        ///  Async   Maps a query to objects
        /// </summary>
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int commandTimeout = CommandTimeout,
            CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();
            dynamic query = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TReturn>(_connectionContainer.Connection, sql, map,
                param, transaction, buffered, splitOn,
                commandTimeout, commandType);
            return query;
        }

        /// <summary>
        ///     Execute a command that returns multiple result sets, and access each in turn
        /// </summary>
        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null,
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();

            return SqlMapper.QueryMultiple(_connectionContainer.Connection, sql, param, transaction, commandTimeout,
                commandType);
        }
        /// <summary>
        ///   Async  Execute a command that returns multiple result sets, and access each in turn
        /// </summary>
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, dynamic param = null, IDbTransaction transaction = null,
            int commandTimeout = CommandTimeout, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _connectionContainer.Transaction;

            OpenConnection();

            return await SqlMapper.QueryMultipleAsync(_connectionContainer.Connection, sql, param, transaction, commandTimeout,
                commandType);
        }

        protected void OpenConnection()
        {
            if (_connectionContainer.Connection != null && _connectionContainer.Connection.State != ConnectionState.Open)
                _connectionContainer.Connection.Open();
        }

        protected void CloseConnection()
        {
            _connectionContainer.Connection?.Close();
        }
    }
}
