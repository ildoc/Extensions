using System.Data;
using System.Data.Common;

namespace Extensions
{
    public static class DbConnectionExtensions
    {
        public static int ExecuteNonQuery(this DbCommand command, string query)
        {
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            return command.ExecuteNonQuery();
        }
    }
}
