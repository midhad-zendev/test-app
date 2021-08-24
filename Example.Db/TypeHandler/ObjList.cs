using Dapper;
using Microsoft.Data.Sqlite;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Data;

namespace Example.Db.TypeHandler
{
    public class ObjList : List<string>
    {

    }
    public class ObjListType : SqlMapper.TypeHandler<ObjList>
    {
        public override void SetValue(IDbDataParameter parameter, ObjList value)
        {
            ((SqliteParameter)parameter).SqliteType = SqliteType.Text;
            parameter.Value = JsonSerializer.SerializeToString(value ?? new ObjList());
        }

        public override ObjList Parse(object value)
        {
            return JsonSerializer.DeserializeFromString<ObjList>(value as string);
        }
    }
}
