using Example.Db.Base;
using Example.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Db.Repositories
{
    public class UsersRepository : BaseRepository
    {
        private const string SqlTableName = "users";
        private const string SqlSelectCommand = "SELECT * FROM " + SqlTableName;
        private const string SqlInsertCommand = "INSERT INTO " + SqlTableName + " (username , password, salt, name , surname) VALUES (@Username, @Password, @Salt, @Name, @Surname)";
        private const string SqlUpdateCommand = "UPDATE " + SqlTableName + " SET name=@Name,surname=@Surname  WHERE id=@Id";
        private const string SqlDeleteCommand = "DELETE FROM " + SqlTableName + " WHERE id=@Id";

        public UsersRepository(ConnectionContainer connectionContainer, bool leaveOpen)
            : base(connectionContainer, leaveOpen)
        {
            if (connectionContainer?.Connection != null)
            {
                LeaveOpen = leaveOpen;
                ConnectionContainer = connectionContainer;
            }
        }

        /// <summary>
        ///     Returns all records from the table.
        ///     Please be aware that any predicate cannot be applied to the returned IEnumurable and it will allways read all
        ///     records.
        /// </summary>
        /// <returns>Read all records from the database table.</returns>
        public IEnumerable<User> All()
        {
            return Query<User>(SqlSelectCommand+" order by id asc").ToList();
        }
        public User GetByUsername(string username)
        {
            return Query<User>(SqlSelectCommand+" where username=@Username", new { Username = username }).FirstOrDefault();
        }
        public int AllCount()
        {
            return Query<int>($"SELECT count(*) FROM {SqlTableName}").FirstOrDefault();
        }

        public User GetById(Int32 id)
        {
            return Query<User>(SqlSelectCommand + " WHERE id=@Id", new { Id = id }).FirstOrDefault();
        }

       
        public void Insert(User model)
        {
            model.Id = Query<int>(SqlInsertCommand + ";select last_insert_rowid()", model).FirstOrDefault();
        }
        public int Delete(Int32 id)
        {
            return Execute(SqlDeleteCommand, new { Id = id });
        }
        public void Insert(List<User> models)
        {
            Execute(SqlInsertCommand, models);
        }
        public void Update(User model)
        {
            Execute(SqlUpdateCommand, model);
        }
        public void UpdateCount(long id, int count)
        {
            Execute($"UPDATE {SqlTableName} SET count=@Count where id=@Id", new { Id = id, Count = count });
        }
     


        /*
        * Async Methods
        */
        public async Task InsertAsync(User model)
        {
            model.Id = (await QueryAsync<int>(SqlInsertCommand + " RETURNING id", model)).FirstOrDefault();
        }
        public async Task<int> DeleteAsync(Int32 id)
        {
            return await ExecuteAsync(SqlDeleteCommand, new { Id = id });
        }
        public async Task InsertAsync(List<User> models)
        {
            await ExecuteAsync(SqlInsertCommand, models);
        }
        public async Task UpdateAsync(User model)
        {
            await ExecuteAsync(SqlUpdateCommand, model);
        }
        
    }
}
