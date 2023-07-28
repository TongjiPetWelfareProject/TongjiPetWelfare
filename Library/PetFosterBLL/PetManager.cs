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
using System.Data;
namespace PetFoster.BLL
{
    public  class PetManager
    {
        public static string user = "\"C##PET\"";
        public static string pwd = "campus";
        public static string db = "localhost:1521/orcl";
        private static string conStr = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + db + ";"; // 替换为实际的数据库连接字符串
        public static void ShowPetProfile(int Limitrow = -1, string Orderby = null)
        {
            DataTable dt =DAL.PetServer.PetInfo(Limitrow, Orderby);
            //调试用
            foreach (DataColumn column in dt.Columns)
            {
                Console.Write("{0,-20}", column.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    if (i == 2)
                    {
                        Console.Write("{0,-30}", JsonHelper.TranslateToCn(row.ItemArray[i].ToString(), "breed"));
                    }
                    else if (i == 5)
                    {
                        Console.Write("{0,-20}", JsonHelper.TranslateToCn(row.ItemArray[i].ToString(), "health_state"));
                    }else if(i==6)
                        Console.Write("{0,-20}", JsonHelper.TranslateToCn(row.ItemArray[i].ToString(), "vaccine"));
                    else
                        Console.Write("{0,-20}", row.ItemArray[i].ToString());
                }

                Console.WriteLine();
            }
        }
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
                    Console.WriteLine($"宠物叫做{Candidate.Pet_Name}\n,品种是{Candidate.Breed}\n,年龄{Candidate.birthdate}\n!");
                connection.Close();
            }

            return con;
        }
        public static void RegisterPet(string Petname, string Breed, string Psize,int Age, string Path = null, string Health_State = "充满活力", bool HaveVaccinated = false)
        {
            DateTime birthDate=DateTime.Now.AddYears(-Age);
            // 读取图像文件
            byte[] BinImage = DAL.PetServer.ConvertImageToByteArray(Petname);

            DAL.PetServer.InsertPet(Petname, Breed, Psize, birthDate, BinImage, Health_State, HaveVaccinated);

        }
        public static void RegisterPet(string Petname, string Breed, string Psize,DateTime birthDate, string Path = null, string Health_State = "充满活力", bool HaveVaccinated = false)
        {
            // 读取图像文件
            byte[] BinImage=DAL.PetServer.ConvertImageToByteArray(Path);

            DAL.PetServer.InsertPet(Petname,Breed,Psize,birthDate,BinImage,Health_State,HaveVaccinated);

        }
    }
}
