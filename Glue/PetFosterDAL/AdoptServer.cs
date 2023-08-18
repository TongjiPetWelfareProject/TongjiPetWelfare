using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFoster.DAL
{
    public class AdoptServer
    {
        public static string conStr = AccommodateServer.conStr;
        public static DataTable PetInfo(decimal Limitrows = -1, string Orderby = null)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                connection.Open();

                string query = "SELECT * FROM pet_profile";
                if (Limitrows > 0)
                    query += $" where rownum<={Limitrows} ";
                if ((Orderby) != null)
                    query += $" order by {Orderby} desc";

                OracleCommand command = new OracleCommand(query, connection);

                OracleDataAdapter adapter = new OracleDataAdapter(command);

                adapter.Fill(dataTable);

                connection.Close();
            }

            //Console.ReadLine();
            return dataTable;
        }
        public static void InsertAdopt(string UID, string PID, DateTime dt, out int errcode)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO adopt (adopter_id, pet_id,apply_date)" +
                    $"VALUES ({UID},{PID},:dt)";
                    command.Parameters.Add("dt", OracleDbType.Date, dt, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                        errcode = 0;
                        Console.WriteLine($"{UID}申请成功，被分配到的宠物是{PID}");
                    }
                    catch (OracleException ex)
                    {
                        errcode = -1;
                        Console.WriteLine("不存在的用户或宠物");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                errcode = -2;
                Console.WriteLine(ex.ToString());
            }
        }
    }
}