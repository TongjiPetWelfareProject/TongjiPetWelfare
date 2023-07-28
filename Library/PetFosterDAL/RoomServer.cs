using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace PetFoster.DAL
{
    public class RoomServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        /// <summary>
        /// 查看每层楼的房源信息，由ShowAvailable(DataTable dt)调用
        /// </summary>
        /// <param name="Limitrows">最多显示的行数</param>
        /// <param name="Orderby">排序的依据（降序）</param>
        /// <returns>返回数据表</returns>
        public static DataTable StoreyInfo()
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM room_avaiable ";

                OracleCommand command = new OracleCommand(query, connection);

                OracleDataAdapter adapter = new OracleDataAdapter(command);

                adapter.Fill(dataTable);

                connection.Close();


            }

            Console.ReadLine();
            return dataTable;
        }
        /// <summary>
        /// 查看房间信息，由ShowProfiles(DataTable dt)调用
        /// </summary>
        /// <param name="Limitrows">最多显示的行数</param>
        /// <param name="Orderby">排序的依据（降序）</param>
        /// <returns>返回数据表</returns>
        public static DataTable RoomInfo(decimal Limitrows = -1, string Orderby = null,bool OnlyAvailable=false)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM room ";
                if (Limitrows > 0)
                    query += $" where rownum<={Limitrows} ";
                else if (OnlyAvailable)
                    query += $"where room_status='N'";
                if (Limitrows > 0&&OnlyAvailable)
                    query += "and room_status='N' ";
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
        /// <summary>
        /// 更改房间信息，由征用/归还房间函数RentARoom(int requiredsize)和打扫房间调用，需要满足
        /// 房间大小大于某一大小
        /// </summary>
        /// <param name="room_status">房间是否被占用</param>
        /// <param name="storey">楼层数</param>
        /// <param name="compartment">房间号</param>
        /// <param name="HaveCleaned">true为打扫，false为退房/租房</param>
        public static void UpdateRoom(short storey, short compartment, bool HaveCleaned=false,string room_status=null)
        {
            // 更改信息
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    
                    if (HaveCleaned)
                    {
                        command.CommandText = "UPDATE room SET cleaning_time=:Cleaning_Time " +
                        "where compartment=:compartment and storey=:storey";
                        command.Parameters.Clear();
                        command.Parameters.Add("Cleaning_Time", OracleDbType.TimeStamp, DateTime.Now, ParameterDirection.Input);
                    }
                    else
                    {
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE room SET room_status='Y' " +
                        $"where compartment={compartment} and storey={storey}";
                        
                    }
                    try
                    {
                        PetData petData = new PetData();
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
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 随机分配空闲的房间
        /// </summary>
        /// <param name="storey"></param>
        /// <param name="compartment"></param>
        public static void AllocateRoom(out short storey,out short compartment)
        {
            bool con = false;
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                Random random = new Random();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from room where room_status='N'"; 
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        double randomDouble = random.NextDouble();
                        storey = reader.GetInt16(2);
                        compartment = reader.GetInt16(3);
                        if (randomDouble > 0.5)
                            continue;
                        return;

                    }
                    storey = -1;
                    compartment = -1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                connection.Close();
            }
        }
    }
}
