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
        public static string conStr = AccommodateServer.conf.GetConnectionString("MyDatabase");

        public static int InsertImage(string FID, string Path)
        {
            byte[] BinImage = PetServer.ConvertImageToByteArray(Path);
            return InsertImage(FID, BinImage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FID">帖子ID</param>
        /// <param name="contents"></param>
        public static int InsertImage(string FID, byte[] image)
        {
            // 添加新行
            try
            {
                using (OracleConnection connection = new OracleConnection(conStr))
                {
                    connection.Open();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO post_images (image_id,post_id,image_data) " +
                        $"VALUES (img_id_seq.NEXTVAL,:post_id,:image_data)"
                        ;
                    command.Parameters.Clear();
                    command.Parameters.Add("post_id", OracleDbType.Varchar2, FID, ParameterDirection.Input);
                    command.Parameters.Add("image_data", OracleDbType.Blob, image, ParameterDirection.Input);
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
        public static List<byte[]> GetImages(int FID)
        {
            List<byte[]> Imgs = new List<byte[]>();
            string getImageQuery = $"SELECT image_data FROM post_images WHERE Post_ID = {FID}";

            using (OracleConnection connection = new OracleConnection(conStr))
            {
                using (OracleCommand getImagesCommand = new OracleCommand(getImageQuery, connection))
                {
                    connection.Open();

                    using (OracleDataReader reader = getImagesCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] postID = (byte[])reader["Image_data"];
                            Imgs.Add(postID);
                        }
                    }
                }
            }

            return Imgs;
        }
    }
}
