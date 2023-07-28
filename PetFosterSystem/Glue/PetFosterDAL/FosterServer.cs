using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
        public static DataTable FosterInfo(string censorStr,decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT fosterer,pet_id,to_char(start_year)||'-'||to_char(start_month)||'-'||to_char(start_day) AS STARTDATE" +
                    ",REMARK FROM foster ";
                if (Limitrows > 0)
                {
                    if (censorStr != "")
                        query += $" where rownum<={Limitrows} and censor_state='{censorStr}'";
                    else
                        query += $" where rownum<={Limitrows}";
                }
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
        public static void UpdateFosterEntry(string UID, string PID,DateTime date,string censor_status)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"UPDATE foster SET censor_status={censor_status}" +
                    $" where user_id={UID} and pet_id={PID} and start_year={date.Year}" +
                    $"and start_month={date.Month} and start_day={date.Day}";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("错误码" + ex.ErrorCode.ToString());

                    throw;
                }
                connection.Close();
            }
        }
        public static bool GetFosterEntries(string UID, string PID)
        {
            bool con = false;
            User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"select *from like_pet where Pet_ID={PID} and User_ID={UID}";
                command.Parameters.Clear();
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        connection.Close();
                        return true;
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理
                    }
                    connection.Close();
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Petname">宠物名</param>
        /// <param name="size">宠物类型进一步细分（eg：大中小型犬）</param>
        /// <param name="dateTime">date1:寄养开始时间</param>
        /// <param name="duration"></param>
        /// <param name="remark"></param>
        /// <returns>错误码</returns>
        public static int InsertFoster(string UID,string PID,DateTime dateTime,int duration,string remark)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = $"INSERT INTO foster (fosterer,pet_id,start_year,start_month,start_day,duration,remark) " +
                        $"VALUES ('{UID}','{PID}',{dateTime.Year},{dateTime.Month},{dateTime.Day},'{duration}','{remark}')";
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{UID}给{PID}在{DateTime.Now}抚养宠物");
                        return 0;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("不存在的用户或宠物");
                        connection.Close();
                        return 2;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
            }
            return 3;
        }
        /// <summary>
        /// 判断对于同一个人foster同一个宠物，那么是否会出现时间区间重叠
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="PID"></param>
        /// <param name="dateTime"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static bool NoOverlapping(string UID, string PID, DateTime dateTime, int duration)
        {
            bool con = false;
            //User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT * FROM foster WHERE pet_id = {PID} AND fosterer = {UID}" +
                    $" AND ( (start_year < {dateTime.Year} AND start_year + duration >= start_year) OR" +
                    $" (start_year = {dateTime.Year} AND start_month < {dateTime.Month} AND start_month + duration >= {dateTime.Month}) OR " +
                    $" (start_year = {dateTime.Year} AND start_month = {dateTime.Month} AND start_day <= {dateTime.Day} AND start_day + duration >= {dateTime.Day}))";
                command.Parameters.Clear();
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        connection.Close();
                        return true;
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理
                    }
                    connection.Close();
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        }
}
