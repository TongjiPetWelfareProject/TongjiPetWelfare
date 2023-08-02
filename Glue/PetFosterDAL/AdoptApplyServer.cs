using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class AdoptApplyServer
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        public static DataTable FosterInfo(string censorStr = "", decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT *" +
                    "FROM adopt_apply ";
                if (Limitrows > 0)
                {
                    if (censorStr != "")
                        query += $" where rownum<={Limitrows} and censor_state='{censorStr}'";
                    else
                        query += $" where rownum<={Limitrows}";
                }
                else
                    query += $" where censor_state='{censorStr}'";
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
        //选择不在申请寄养或领养中并排除已经被寄养或领养的宠物
        public static int GetRandomPet(string species)
        {
            int? exist = 0;
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"SELECT pet_id FROM ( SELECT *  FROM pet  " +
                    $"WHERE species = '{species}'  ORDER BY DBMS_RANDOM.VALUE) WHERE pet_id " +
                    $"NOT IN(SELECT adopter_id FROM adopt) AND  pet_id NOT IN " +
                    $"(SELECT pet_id FROM foster where censor_state = 'legitimate' or censor_state = " +
                    $"'to be censored') AND ROWNUM <= 1";
                try
                {
                    exist = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                    if (exist == null)
                        return -1;//宠物都没空
                    else
                        return Convert.ToInt32(exist);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    connection.Close();
                    return -2;//执行异常
                }

            }
        }
        public static void UpdateAdoptEntry(string UID, string censor_status)
        {
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = $"UPDATE adopt_apply SET censor_state='{censor_status}'" +
                    $" where adopter_id={UID}";
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"用户:{UID}的领养申请通过状态为{censor_status}");
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("错误码" + ex.ErrorCode.ToString());

                    throw;
                }
                connection.Close();
            }
        }
        public static void InsertAdoptApply(string UID, string species, bool gender, bool pet_exp, bool long_term_care,
            bool w_to_treat, decimal d_care_h, string P_Caregiver, decimal f_popul, bool be_children, bool accept_vis)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO adopt_apply (adopter_id, species,adopter_gender,pet_experience" +
                       ",long_term_care,willing_to_treat,daily_care_hours,primary_caregiver,family_population,has_children,accept_visits) " +
                       $"VALUES ({UID},'{species}',:gender,:p_exp,:lt_care,:w_t_treat,{d_care_h},'{P_Caregiver}',{f_popul},:h_child,:a_vis)";
                    command.Parameters.Add("gender", OracleDbType.Varchar2, gender ? 'M' : 'F', ParameterDirection.Input);
                    command.Parameters.Add("p_exp", OracleDbType.Varchar2, pet_exp ? 'Y' : 'N', ParameterDirection.Input); ;
                    command.Parameters.Add("lt_care", OracleDbType.Varchar2, long_term_care ? 'Y' : 'N', ParameterDirection.Input);
                    command.Parameters.Add("w_t_treat", OracleDbType.Varchar2, w_to_treat ? 'Y' : 'N', ParameterDirection.Input);
                    command.Parameters.Add("h_child", OracleDbType.Varchar2, be_children ? 'Y' : 'N', ParameterDirection.Input);
                    command.Parameters.Add("a_vis", OracleDbType.Varchar2, accept_vis ? 'Y' : 'N', ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{UID}请求申请抚养宠物");

                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("不存在的用户或宠物");
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