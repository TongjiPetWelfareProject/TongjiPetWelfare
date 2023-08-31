using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//不需要查询图片！！！
namespace PetFoster.DAL
{
    public class PostImagesServer
    {
        public static string conStr = AccommodateServer.conStr;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FID">帖子ID</param>
        /// <param name="contents"></param>
        public static int InsertImage(string FID, string url)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO post_images (image_id,post_id,image_path) " +
                        $"VALUES (img_id_seq.NEXTVAL,:post_id,:image_data)"
                        ;
                    command.Parameters.Clear();
                    command.Parameters.Add("post_id", OracleDbType.Varchar2, FID, ParameterDirection.Input);
                    command.Parameters.Add("image_data", OracleDbType.Varchar2, url, ParameterDirection.Input);
                    try
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT img_id_seq.CURRVAL FROM DUAL";
                        int ImgId = Convert.ToInt32(command.ExecuteScalar());
                        return ImgId;
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine("错误码" + ex.ErrorCode.ToString());
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.ToString());
            }
            return -1;
        }
        public static List<string> GetImages(int FID)
        {
            List<string> Imgs = new List<string>();
            string getImageQuery = $"SELECT image_path FROM post_images WHERE Post_ID = {FID}";

            using (OracleConnection connection = new OracleConnection(conStr))
            {
                using (OracleCommand getImagesCommand = new OracleCommand(getImageQuery, connection))
                {
                    connection.Open();

                    using (OracleDataReader reader = getImagesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string postPath = reader["Image_path"].ToString();
                            Imgs.Add(postPath);
                        }
                    }
                }
            }

            return Imgs;
        }
    }
}
