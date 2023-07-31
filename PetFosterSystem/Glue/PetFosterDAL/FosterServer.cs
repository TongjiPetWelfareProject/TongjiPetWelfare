using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class FosterServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        /// <summary>
        /// 查看领养信息（RQX的我的领养），由ShowProfiles(DataTable dt)调用
        /// </summary>
        /// <param name="Limitrows">最多显示的行数</param>
        /// <param name="Orderby">排序的依据（降序）</param>
        /// <returns>返回数据表</returns>
        public static DataTable FosterInfo(decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM foster_window ";
                if (Limitrows > 0)
                    query += $" where rownum<={Limitrows} ";
                if ((Orderby) != null)
                    query += $" order by {Orderby} desc";

                OracleCommand command = new OracleCommand(query, connection);

                OracleDataAdapter adapter = new OracleDataAdapter(command);

                adapter.Fill(dataTable);

                connection.Close();


            }

            Console.ReadLine();
            return dataTable;
        }
    }
}
