using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Extensions
{
    public static class DbConnectionExtensions
    {
        public static Task<int> ExecuteNonQueryAsync(this DbCommand command, string query)
        {
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            return command.ExecuteNonQueryAsync();
        }
    }
}
