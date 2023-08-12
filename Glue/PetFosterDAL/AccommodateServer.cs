using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PetFoster.DAL
{
    internal class AccommodateServer
    {
        public static IConfiguration conf=new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        public static string conStr = conf.GetConnectionString("MyDatabase");
        public static void InsertAccommodate(string UID, string PID,short storey,short compartment)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO accommodate (owner_id, pet_id,storey,compartment) " +
                        "VALUES (:user_id,:pet_id,:storey,:compartment)";
                    command.Parameters.Clear();
                    command.Parameters.Add("user_id", OracleDbType.Varchar2, UID, ParameterDirection.Input);
                    command.Parameters.Add("pet_id", OracleDbType.Varchar2, PID, ParameterDirection.Input);
                    command.Parameters.Add("user_id", OracleDbType.Int16, storey, ParameterDirection.Input);
                    command.Parameters.Add("pet_id", OracleDbType.Int16, compartment, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{UID}开始申请{PID}居住于{storey}-{compartment}室");

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
