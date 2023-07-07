using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class PetServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        private static byte[] GetRandomAvatar(string imagePath)
        {
            // 获取随机的 avatar
            // ...
            byte[] imageBytes = ConvertImageToByteArray(imagePath);
            return imageBytes;
        }


        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageData;

            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    imageData = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }

            return imageData;
        }
        public static DataTable PetInfo(decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT pet_id,pet_name,breed,TRUNC(MONTHS_BETWEEN(CURRENT_TIMESTAMP, birthdate) / 12) AS age,health_state,vaccine,read_num,like_num,collect_num FROM pet";
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
        /// <summary>
        /// 用户登录时匹配用户信息，如果为0，说明密码错误,否则密码正确
        /// </summary>
        /// <param name="user">用户行</param>
        /// <returns></returns>
        public static Pet SelectPet(string PID)
        {
            bool con = false;
            Pet pet = new Pet();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select *from pet where Pet_ID=:PID";
                command.Parameters.Clear();
                command.Parameters.Add("user_id", OracleDbType.Varchar2, PID, ParameterDirection.Input);
                try
                {
                    OracleDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // 访问每一行的数据
                        // 其他列..
                        pet.Pet_ID = reader["Pet_ID"].ToString();
                        pet.Pet_Name = reader["Pet_Name"].ToString();
                        pet.Breed = reader["Breed"].ToString();
                        pet.birthdate = Convert.ToDateTime(reader["BIRTHDATE"]);
                        pet.Avatar = (byte[])(reader["Avatar"]);
                        pet.Read_Num = Convert.ToDecimal(reader["Read_Num"]);
                        pet.Like_Num = Convert.ToDecimal(reader["Like_Num"]);
                        pet.Collect_Num = Convert.ToDecimal(reader["Collect_Num"]);
                        // 执行你的逻辑操作，例如将数据存储到自定义对象中或进行其他处理

                    }
                    if (pet.Pet_ID == "-1")
                        throw new Exception("不存在的宠物！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                connection.Close();
            }

            return pet;
        }
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
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
        static bool IsValidStatus(string status)
        {
            // 读取配置文件并加载状态
            List<string> statuses = new List<string>();
            string configFile = "config/account_status.txt";
            string[] lines = File.ReadAllLines(configFile);
            statuses.AddRange(lines);
            if (statuses.Contains(status))
                return true;
            else
            {
                throw new Exception(status + "状态不合法！");
            }
        }
        /// <summary>
        /// 插入宠物信息
        /// </summary>
        /// <param name="Petname">宠物名</param>
        /// <param name="Breed">宠物品种，必须符合JSON中列出的几种类别</param>
        /// <param name="birthDate">出生日期，由于年龄为派生类，不好维护，因此改用出生日期</param>
        /// <param name="Avatar">图像，用BLOB存储</param>
        /// <param name="Health_State">健康状态</param>
        /// <param name="HaveVaccinated">是否接种疫苗</param>
        public static void InsertPet(string Petname, string Breed ,DateTime birthDate,byte[]Avatar=null, string Health_State="充满活力",bool HaveVaccinated=false)
        {
            string vaccine = "";
            if (HaveVaccinated)
                vaccine = HaveVaccinated ? "Y" : "N";
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO pet (pet_id,pet_name, breed, birthdate, avatar, health_state, vaccine) " +
                        "VALUES (pet_id_seq.NEXTVAL,:pet_name,:breed, :birthdate, :avatar, :health_state, :vaccine)";
                    command.Parameters.Clear();
                    command.Parameters.Add("pet_name", OracleDbType.Varchar2, Petname, ParameterDirection.Input);
                    command.Parameters.Add("breed", OracleDbType.Varchar2, Breed, ParameterDirection.Input);
                    command.Parameters.Add("birthdate", OracleDbType.Date, birthDate, ParameterDirection.Input);
                    command.Parameters.Add("avatar", OracleDbType.Blob, Avatar, ParameterDirection.Input);
                    command.Parameters.Add("health_state", OracleDbType.Varchar2, Health_State, ParameterDirection.Input);
                    command.Parameters.Add("vaccine", OracleDbType.Varchar2, vaccine, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("错误码" + ex.ErrorCode.ToString());

                        throw;
                    }
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
