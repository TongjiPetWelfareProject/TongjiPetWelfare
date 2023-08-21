using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using PetFoster.Model;
using static PetFoster.Model.PetData;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace PetFoster.DAL
{
    public class UserServer
    {
        public static string conStr = AccommodateServer.conStr;
        /// <summary>
        /// 查看用户信息，由ShowProfiles(DataTable dt)调用
        /// 注意用户的密码不能用明文存储，最起码的要求密码不能在客户端显示！！！
        /// </summary>
        /// <param name="Limitrows">最多显示的行数</param>
        /// <param name="Orderby">排序的依据（降序）</param>
        /// <returns>返回数据表</returns>
        public static DataTable UserInfo(decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT user_id,user_name,phone_number,account_status,address FROM user2 where role='User'";
            return DBHelper.ShowInfo(query, Limitrows, Orderby);
        }

        /// <summary>
        /// 用户登录时匹配用户信息，如果为0，说明密码错误,否则密码正确
        /// </summary>
        /// <param name="user">用户行</param>
        /// <returns></returns>
        public static User GetUser(string UID, string pwd, bool IsAdmin = false)
        {
            bool con = false;
            User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = "select *from user2 where User_ID=:user_id";
                command.Parameters.Clear();
                command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // 访问每一行的数据
                        // 其他列..
                        user1.User_ID = reader["User_ID"].ToString();
                        user1.User_Name = reader["User_Name"].ToString();
                        user1.Account_Status = reader["Account_Status"].ToString();
                        user1.Address = reader["Address"].ToString();
                        user1.Password = reader["Password"].ToString();
                        user1.Phone_Number = reader["Phone_Number"].ToString();
                        user1.Role = reader["Role"].ToString();
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理

                    }
                    if (user1.User_ID == "-1")
                        throw new Exception("不存在的用户，请注册新用户！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }

            return user1;
        }
        public static Profile GetStatistics(int UID, out int err)
        {
            bool con = false;
            err = 0;
            Profile profile = new Profile(-1, -1);
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                profile.getlikes = -1;
                command.CommandText = $"select total_like,clicks from user_profile where User_ID={UID}";
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // 访问每一行的数据
                        // 其他列..
                        profile.getlikes = Convert.ToInt32(reader["Total_Like"]);
                        profile.getclicks = Convert.ToInt32(reader["Clicks"]);
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理

                    }
                    if (profile.getlikes == -1)
                    {
                        err = -1;
                        throw new Exception("你要查找的用户不存在！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    err = -2;//查询过程异常
                }
                connection.Close();
            }

            return profile;
        }
        public static User GetUserByTel(string Tel, string pwd, bool IsAdmin = false)
        {
            bool con = false;
            User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;

                command.CommandText = $"select *from user2 where Phone_Number={Tel}";
                try
                {
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // 访问每一行的数据
                        // 其他列..
                        user1.User_ID = reader["User_ID"].ToString();
                        user1.User_Name = reader["User_Name"].ToString();
                        user1.Account_Status = reader["Account_Status"].ToString();
                        user1.Address = reader["Address"].ToString();
                        user1.Password = reader["Password"].ToString();
                        user1.Phone_Number = reader["Phone_Number"].ToString();
                        user1.Role = reader["Role"].ToString();
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理

                    }
                    if (user1.User_ID == "-1")
                        throw new Exception("不存在的用户，请注册新用户！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }

            return user1;
        }
        public static string GetRole(string UID)
        {
            bool con = false;
            User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select role from user2 where User_ID=:user_id";
                command.Parameters.Clear();
                command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                try
                {
                    string role = command.ExecuteScalar() as string;
                    if (role == null)
                        return "Unknown";
                    else
                        return role;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "Error";
                }
            }
        }
        public static string GetName(string UID)
        {
            bool con = false;
            User user1 = new User();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select user_name from user2 where User_ID=:user_id";
                command.Parameters.Clear();
                command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                try
                {
                    string username = command.ExecuteScalar() as string;
                    if (username == null)
                        return "User"+UID;
                    else
                        return username;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "Error";
                }
            }
        }
        static bool IsValidAddress(string address)
        {
            // 读取配置文件并加载地址
            List<string> addresses = new List<string>();
            string configFile = "config/addresses.txt";
            string[] lines = File.ReadAllLines(configFile);
            addresses.AddRange(lines);
            if (addresses.Contains(address))
                return true;
            else
            {
                throw new Exception(address + "地址不合法！");
            }
        }
        /// <summary>
        /// 校验信息并注册
        /// </summary>
        /// <param name="Username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="Address">地址</param>
        public static string InsertUser(string Username, string pwd, string phoneNumber, string Address = "Beijing")
        {
            string UID = "-1";
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    string account_status = "In Good Standing";
                    command.CommandText = "INSERT INTO user2 (user_id, user_name, password, phone_number, account_status, address) " +
                        "VALUES (user_id_seq.NEXTVAL, :user_name, :password, :phone_number, :account_status, :address)";
                    command.Parameters.Clear();
                    command.Parameters.Add("user_name", OracleDbType.Varchar2, Username, ParameterDirection.Input);
                    command.Parameters.Add("password", OracleDbType.Varchar2, pwd, ParameterDirection.Input);
                    command.Parameters.Add("phone_number", OracleDbType.Varchar2, phoneNumber, ParameterDirection.Input);
                    command.Parameters.Add("account_status", OracleDbType.Varchar2, account_status, ParameterDirection.Input);
                    command.Parameters.Add("address", OracleDbType.Varchar2, Address, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "select user_id_seq.CURRVAL from dual";
                        UID = command.ExecuteScalar().ToString();
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
            return UID;
        }

        public static bool DeleteUser(string UID)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 执行删除操作
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "delete from user2 where User_ID= :User_ID";
                command.Parameters.Clear();
                command.Parameters.Add("User_ID", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                try
                {
                    command.ExecuteNonQuery();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static void UpdateUser(string UID, string Username, string pwd, string phoneNumber, string Address = "Beijing", string account_status = "In Good Standing")
        {
            // 更改信息
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE user2 SET user_name=:user_name, password=:password, phone_number=:phone_number, " +
                        "account_status=:account_status, address=:address where user_id=:user_id";
                    command.Parameters.Clear();
                    command.Parameters.Add("user_name", OracleDbType.Varchar2, Username, ParameterDirection.Input);
                    command.Parameters.Add("password", OracleDbType.Varchar2, pwd, ParameterDirection.Input);
                    command.Parameters.Add("phone_number", OracleDbType.Varchar2, phoneNumber, ParameterDirection.Input);
                    command.Parameters.Add("account_status", OracleDbType.Varchar2, account_status, ParameterDirection.Input);
                    command.Parameters.Add("address", OracleDbType.Varchar2, Address, ParameterDirection.Input);
                    command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
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
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
