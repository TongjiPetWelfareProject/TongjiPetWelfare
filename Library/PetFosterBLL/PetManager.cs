using Oracle.ManagedDataAccess.Client;
using PetFoster.DAL;
using PetFoster.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetFoster.Model.PetData;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
namespace PetFoster.BLL
{
    public  class PetManager
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        public static bool ViewProfile(int PetID,out Pet Candidate)
        {
            string PID=PetID.ToString();
            bool con = false;
            using (OracleConnection connection = new OracleConnection(conStr))
            {
                // 连接对象将在 using 块结束时自动关闭和释放资源
                // 在此块中执行数据操作
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                Candidate = DAL.PetServer.SelectPet(PID);
                if (Candidate.Pet_ID == "-1")
                    Console.WriteLine("PID不存在！");
                else
                    Console.WriteLine($"宠物叫做{Candidate.Pet_Name}\n,品种是{Candidate.Breed}\n,年龄{Candidate.Age}\n!");
                connection.Close();
            }

            return con;
        }

        
    }
}
