using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class BulletinServer
    {
        public static string conStr = AccommodateServer.conStr;
        //用户端公告主界面
        public static DataTable BulletinInfo(string text)
        {
            string[] keywords = text.Split(',', ' ', '和', '&', '或', '与', '是', '.', '\\', '/');
            DataTable dataTable = new DataTable();
            string query = "SELECT bulletin_id,heading,published_time " +
                    "from bulletin where ";
            foreach (string keyword in keywords)
            {
                query += $" heading like '%{keyword}%' or";
            }
            if (query.EndsWith("or"))
                query = query.Substring(0, query.Length - 2);
            query += " order by published_time desc";
            return DBHelper.ShowInfo(query);
        }
        public static Bulletin SelectBulletin(string BID)
        {
            Bulletin btin = new Bulletin();
            btin.Id = "B-1";
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"select *from  bulletin where bulletin_id='{BID}'";
                try
                {
                    OracleDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // 访问每一行的数据
                        // 其他列..
                        btin.Heading = reader["heading"].ToString();
                        btin.Id = reader["bulletin_id"].ToString();
                        btin.published_date = Convert.ToDateTime(reader["published_time"]);
                        btin.Content = reader["bulletin_contents"].ToString();
                        btin.ReadCount = Convert.ToInt32(reader["read_count"]);
                        btin.EmployeeID = Convert.ToInt32(reader["employee_id"]);
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理

                    }
                    if (btin.Id == "B-1")
                        throw new Exception("不存在的公告！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                connection.Close();
            }

            return btin;
        }

        public static List<Bulletin> GetAllBulletins()
        {
            List<Bulletin> bulletins = new List<Bulletin>();

            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT bulletin_id, heading, published_time, bulletin_contents, read_count " +
                               "FROM bulletin " +
                               "ORDER BY published_time DESC";

                OracleCommand command = new OracleCommand(query, connection);

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Bulletin bulletin = new Bulletin
                        {
                            Id = reader["bulletin_id"].ToString(),
                            Heading = reader["heading"].ToString(),
                            published_date = Convert.ToDateTime(reader["published_time"]),
                            Content = reader["bulletin_contents"].ToString(),
                            ReadCount = Convert.ToInt32(reader["read_count"])
                        };

                        bulletins.Add(bulletin);
                    }
                }

                connection.Close();
            }

            return bulletins;
        }
    }
}
